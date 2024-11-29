using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace _2DPuzzle
{
    public class UpdateManager
    {
        private static UpdateManager _instance;

        private static readonly object _lock = new object();

        public static UpdateManager GetInstance()
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
                        _instance = new UpdateManager();
                    }
                }
            }
            return _instance;
        }

        public float deltaTime = 0;

        public List<EntityComponent> entityComponents = null;

        public UpdateManager()
        {
            entityComponents = new List<EntityComponent>();
        }

        public void Clear()
        {
            entityComponents.Clear();
        }

        public void RegisterComponent(EntityComponent inComponent)
        {
            entityComponents.Add(inComponent);
        }

        public void UnregisterComponent(EntityComponent inComponent)
        {
            entityComponents.Remove(inComponent);
        }

        // Return true if game must continue
        public bool Update(GameTime inGameTime)
        {
            if (/*InputManager.GetInstance().IsGamepadButtonDown(InputManager.GamepadButton.Back) || */InputManager.GetInstance().IsKeyDown(Keys.Escape))
                return false;

            deltaTime = (float)inGameTime.ElapsedGameTime.TotalSeconds;

            for(int currentComponentIndex = 0;  currentComponentIndex < entityComponents.Count; currentComponentIndex++)
            {
                if (entityComponents[currentComponentIndex].canUpdate && entityComponents[currentComponentIndex].enabled)
                {
                    entityComponents[currentComponentIndex].Update(inGameTime);
                }
            }

            return true;
        }
    }
}
