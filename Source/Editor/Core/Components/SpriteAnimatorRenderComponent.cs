using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace _2DPuzzle
{
    public class SpriteAnimatorRenderComponent : RenderComponent
    {
        public List<Texture2D> sprites = null;
        public bool loop = true;
        public int frameLength = 5;

        protected int _currentIndex = 0;
        protected int _currentFrameLenghtIndex = 0;

        public SpriteAnimatorRenderComponent(Entity inOwner, string inSpritePath, int inFrameNumber, bool inLoop) : base(inOwner)
        {
            loop = inLoop;
            sprites = new List<Texture2D>();
            for(int spriteIndex = 0; spriteIndex < inFrameNumber; spriteIndex++)
            {
                sprites.Add(RenderManager.GetInstance().content.Load<Texture2D>(inSpritePath + (spriteIndex + 1)));
            }
        }

        public override void Render(GameTime inGameTime)
        {
            base.Render(inGameTime);

            RenderManager.GetInstance().totalBatch++;
            RenderManager.GetInstance().spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: RenderManager.GetInstance().screenScaleMatrix);
            //RenderManager.GetInstance().spriteBatch.Draw(sprites[_currentIndex], _transformComponent.position, null, Color.White, owner.transformComponent.rotation, Vector2.Zero, owner.transformComponent.scale, new SpriteEffects(), 0);
            RenderManager.GetInstance().spriteBatch.Draw(sprites[_currentIndex], _transformComponent.position, Color.White);
            RenderManager.GetInstance().spriteBatch.End();

            _currentFrameLenghtIndex++;
            if(_currentFrameLenghtIndex >= frameLength)
            {
                _currentFrameLenghtIndex = 0;
                _currentIndex++;
                if(loop && _currentIndex >= sprites.Count)
                {
                    _currentIndex = 0;
                }
            }

        }
    }
}
