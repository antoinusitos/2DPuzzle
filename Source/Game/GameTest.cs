﻿namespace _2DPuzzle
{
    public class GameTest : GameBase
    {
        protected override void Initialize()
        {
            base.Initialize();

            _levelManager.AddLevel(new LevelTest());
        }
    }
}