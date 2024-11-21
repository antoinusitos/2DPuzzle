using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace _2DPuzzle
{
    public class SpriteAnimatorRender
    {
        [JsonIgnore]
        public List<Texture2D> sprites = null;
        public string spritePath = "";
        public bool loop = true;
        public int frameLength = 5;

        protected int _currentIndex = 0;
        protected int _currentFrameLenghtIndex = 0;

        public SpriteAnimatorRender(string inSpritePath, int inFrameNumber, bool inLoop)
        {
            loop = inLoop;
            sprites = new List<Texture2D>();
            spritePath = inSpritePath;

            if(inFrameNumber == 1)
            {
                sprites.Add(RenderManager.GetInstance().content.Load<Texture2D>(inSpritePath));
                return;
            }

            for (int spriteIndex = 0; spriteIndex < inFrameNumber; spriteIndex++)
            {
                sprites.Add(RenderManager.GetInstance().content.Load<Texture2D>(inSpritePath + (spriteIndex + 1)));
            }
        }

        public void UpdateAnimator()
        {
            if(sprites.Count == 1)
            {
                return;
            }

            _currentFrameLenghtIndex++;
            if (_currentFrameLenghtIndex >= frameLength)
            {
                _currentFrameLenghtIndex = 0;
                _currentIndex++;
                if (loop && _currentIndex >= sprites.Count)
                {
                    _currentIndex = 0;
                }
            }
        }

        public int GetCurrentIndex()
        { 
            return _currentIndex; 
        }

        public Texture2D GetCurrentSprite()
        {
            return sprites[_currentIndex];
        }
    }
}
