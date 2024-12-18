﻿using System.Collections.Generic;

namespace _2DPuzzle
{
    public class LevelManager
    {
        private static LevelManager _instance;

        private static readonly object _lock = new object();

        public static LevelManager GetInstance()
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
                        _instance = new LevelManager();
                    }
                }
            }
            return _instance;
        }

        private List<Level> _levels = null;

        public Level currentLevel { get; private set; } = null;

        public void InitializeManager()
        {
            _levels = new List<Level>();
            currentLevel = new Level();
        }

        public Level[] GetLevelsArray()
        {
            return _levels.ToArray();
        }

        public void NewLevel()
        {
            if (currentLevel != null)
            {
                currentLevel.UninitializeLevel();
                currentLevel = null;

                RenderManager.GetInstance().Clear();
                UpdateManager.GetInstance().Clear();
            }

            currentLevel = new Level();
        }

        public void AddLevel(Level inLevel)
        {
            _levels.Add(inLevel);

            if(currentLevel == null)
            {
                inLevel.InitializeLevel();
                currentLevel = inLevel;
            }
        }

        public void LoadLevel(string inLevelName)
        {
            Debug.Log("Loading level " + inLevelName);

            if (currentLevel != null)
            {
                currentLevel.UninitializeLevel();
                currentLevel = null;
                RenderManager.GetInstance().Clear();
                UpdateManager.GetInstance().Clear();
            }
            
            /*currentLevel = _levels[levelIndex];
            currentLevel.InitializeLevel();
            currentLevel.Start();
            return;*/

            currentLevel = new Level();
            currentLevel.Load(inLevelName);
            currentLevel.Start();
        }

        public void SaveLevel()
        {
            currentLevel.Save();
        }
    }
}
