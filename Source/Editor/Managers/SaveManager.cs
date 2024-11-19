using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace _2DPuzzle
{
    [Serializable]
    public class MyJsonDictionary<K, V> : ISerializable
    {
        Dictionary<K, V> dict = new Dictionary<K, V>();

        public MyJsonDictionary() { }

        protected MyJsonDictionary(SerializationInfo info, StreamingContext context)
        {
            dict = (Dictionary<K,V>)info.GetValue("foo", typeof(Dictionary<K,V>));

            dict.OnDeserialization(null);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("foo", dict, typeof(Dictionary<K,V>));
        }

        public void Add(K key, V value)
        {
            dict.Add(key, value);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var pair in dict)
                sb.Append(pair.Key).Append(" : ").Append(pair.Value).AppendLine();
            return sb.ToString();
        }

        public V this[K index]
        {
            set { dict[index] = value; }
            get { return dict[index]; }
        }

        public bool ContainsKey(K inkey)
        {
            foreach (K key in dict.Keys)
            {
                if(key.Equals(inkey))
                {
                    return true;
                }
            }

            return false;
        }
    }

    [Serializable]
    public class SavedData
    {
        public MyJsonDictionary<string, bool> savedBool = new MyJsonDictionary<string, bool>();
        public MyJsonDictionary<string, int> savedInt = null;
        public MyJsonDictionary<string, float> savedFloat = null;
    }

    public class SaveManager
    {
        private static SaveManager _instance;

        private static readonly object _lock = new object();

        public static SaveManager GetInstance()
        {
            // This conditional is needed to prevent threads stumbling over the
            // lock once the instance is ready.
            if (_instance == null)
            {
                // Now, imagine that the program has just been launched. Since
                // there's no Singleton instance yet, multiple threads can
                // simultaneously pass the previous conditional and reach this
                // point almost at the same time. The first of them will acquire
                // lock and will proceed further, while the rest will wait here.
                lock (_lock)
                {
                    // The first thread to acquire the lock, reaches this
                    // conditional, goes inside and creates the Singleton
                    // instance. Once it leaves the lock block, a thread that
                    // might have been waiting for the lock release may then
                    // enter this section. But since the Singleton field is
                    // already initialized, the thread won't create a new
                    // object.
                    if (_instance == null)
                    {
                        _instance = new SaveManager();
                    }
                }
            }
            return _instance;
        }

        private SavedData savedData = null;

        private const string saveFileName = "PlayerPref.json";

        public void InitializeManager()
        {
            savedData = new SavedData()
            {
                savedBool = new MyJsonDictionary<string, bool>(),
                savedInt = new MyJsonDictionary<string, int>(),
                savedFloat = new MyJsonDictionary<string, float>()
            };
        }

        public void SaveBool(string inName, bool inValue)
        {
            if(savedData.savedBool.ContainsKey(inName))
            {
                savedData.savedBool[inName] = inValue;
                return;
            }

            savedData.savedBool.Add(inName, inValue);
        }

        public bool LoadBool(string inName)
        {
            if (savedData.savedBool.ContainsKey(inName))
            {
                return savedData.savedBool[inName];
            }

            Debug.LogWarning("No bool called " + inName + " to load.");
            return false;
        }

        public void SaveInt(string inName, int inValue)
        {
            if (savedData.savedInt.ContainsKey(inName))
            {
                savedData.savedInt[inName] = inValue;
                return;
            }

            savedData.savedInt.Add(inName, inValue);
        }

        public int LoadInt(string inName)
        {
            if (savedData.savedInt.ContainsKey(inName))
            {
                return savedData.savedInt[inName];
            }

            Debug.LogWarning("No int called " + inName + " to load.");
            return -1;
        }

        public void SaveFloat(string inName, float inValue)
        {
            if (savedData.savedFloat.ContainsKey(inName))
            {
                savedData.savedFloat[inName] = inValue;
                return;
            }

            savedData.savedFloat.Add(inName, inValue);
        }

        public float LoadFloat(string inName)
        {
            if (savedData.savedFloat.ContainsKey(inName))
            {
                return savedData.savedFloat[inName];
            }

            Debug.LogWarning("No float called " + inName + " to load.");
            return -1;
        }

        public void SaveAll()
        {
            var formatter = new BinaryFormatter();
            using var stream = new FileStream(saveFileName, FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, savedData);
        }

        public void LoadAll()
        {
            var formatter = new BinaryFormatter();
            using var stream = new FileStream(saveFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            savedData = (SavedData)formatter.Deserialize(stream);
        }
    }
}