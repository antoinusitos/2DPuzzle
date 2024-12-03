using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace _2DPuzzle
{
    public class ContentManager
    {
        private static ContentManager _instance;

        private static readonly object _lock = new object();

        private Dictionary<string, Texture2D> spritesDictionary = new Dictionary<string, Texture2D>();

        public static ContentManager GetInstance()
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
                        _instance = new ContentManager();
                    }
                }
            }
            return _instance;
        }

        public void LoadContent()
        {
            spritesDictionary.Add("Idle", RenderManager.GetInstance().content.Load<Texture2D>("Idle"));
            spritesDictionary.Add("TileTest", RenderManager.GetInstance().content.Load<Texture2D>("TileTest"));
        }

        public Texture2D GetSprite(string inName)
        {
            if(spritesDictionary.ContainsKey(inName))
            {
                return spritesDictionary[inName];
            }

            return null;
        }
    }
}
