using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;

namespace _2DPuzzle
{
    [Serializable]
    public class SavedData
    {
        public Dictionary<string, bool> savedBool = null;
        public Dictionary<string, int> savedInt = null;
        public Dictionary<string, float> savedFloat = null;
        public Dictionary<string, string> savedString = null;
    }

    public class LevelEntitySave
    {
        public string name;
        public uint uniqueID;
        public List<ComponentSave> componentsSaved;
    }

    //Use this to save levels
    public class LevelSave
    {
        public string name;
        public List<LevelEntitySave> entitiesSaved;
        public List<EntityCleanSave> entitiesCleanSaved;
    }

    //Use this to save components
    public class ComponentSave
    {
		public string componentType;
        public SavedData saveData;
        public string componentUniqueID;
    }

    //Use this to save prefab or level entities that are changed
    public class EntitySave
    {
        public string name;
		public List<ComponentSave> componentsSaved;
    }

    //Use this to save level entities that are not changed
    public class EntityCleanSave
    {
        public string name;
        public uint uniqueID;
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
                savedBool = new Dictionary<string, bool>(),
                savedInt = new Dictionary<string, int>(),
                savedFloat = new Dictionary<string, float>()
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

        public void SaveEntity(Entity inEntity)
        {
            EntitySave entitySave = inEntity.GetSaveData();

            string entityToSave = JsonConvert.SerializeObject(entitySave, Formatting.Indented,
            new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            File.WriteAllText("Prefabs/" + inEntity.name + ".json", entityToSave);
        }

        public void LoadEntity(ref Entity inEntity, string inEntityName)
        {
            string jsonString = File.ReadAllText("Prefabs/" + inEntityName + ".json");
            EntitySave entitySave = JsonConvert.DeserializeObject<EntitySave>(jsonString);
            inEntity.name = entitySave.name;
            for(int componentIndex = 0; componentIndex < entitySave.componentsSaved.Count; componentIndex++)
            {
                Type type = Type.GetType(entitySave.componentsSaved[componentIndex].componentType);
                object o = Activator.CreateInstance(type);
                if(type.IsSubclassOf(typeof(EntityComponent)))
                {
                    EntityComponent entityComponent = (EntityComponent)o;
                    entityComponent.uniqueID = uint.Parse(entitySave.componentsSaved[componentIndex].componentUniqueID);
                    entityComponent.owner = inEntity;
                    entityComponent.LoadSavedData(entitySave.componentsSaved[componentIndex].saveData);
                    inEntity.components.Add(entityComponent);
                }
                else if (type == typeof(AnimationState))
                {
                    AnimationState animationState = (AnimationState)o;
                    animationState.uniqueID = uint.Parse(entitySave.componentsSaved[componentIndex].componentUniqueID);
                    AnimatorComponent animatorComponent = inEntity.GetComponent<AnimatorComponent>();
                    animationState.parentAnimatorComponent = animatorComponent;
                    animationState.parentStateMachine = animatorComponent;
                    animationState.LoadSavedData(entitySave.componentsSaved[componentIndex].saveData);
                    if(animatorComponent.currentState == null)
                    {
                        animatorComponent.SetStartingState(animationState);
                    }
                    animatorComponent.Start();
                    animatorComponent.allStates.Add(animationState);
                }
                else if (type == typeof(StateMachineTransition))
                {
                    StateMachineTransition stateMachineTransition = (StateMachineTransition)o;
                    stateMachineTransition.uniqueID = uint.Parse(entitySave.componentsSaved[componentIndex].componentUniqueID);
                    AnimatorComponent animatorComponent = inEntity.GetComponent<AnimatorComponent>();
                    stateMachineTransition.parentStateMachine = animatorComponent;
                    stateMachineTransition.LoadSavedData(entitySave.componentsSaved[componentIndex].saveData);
                    animatorComponent.allTransitions.Add(stateMachineTransition);
                }
            }
            inEntity.transformComponent = inEntity.GetComponent<TransformComponent>();
        }

        public void SaveGame()
        {
            // Use this to save player stats in player prefs
        }

        public void LoadGame()
        {
            // Use this to load player stats in player prefs
        }
    }
}
