using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2DPuzzle
{
    public class SpriteAnimatorRenderComponent : RenderComponent
    {
        public SpriteAnimatorRender spriteAnimatorRender = null;

        public SpriteAnimatorRenderComponent(Entity inOwner, string inSpritePath, int inFrameNumber, bool inLoop) : base(inOwner)
        {
            spriteAnimatorRender = new SpriteAnimatorRender(inSpritePath, inFrameNumber, inLoop);
        }

        public override void Render(GameTime inGameTime)
        {
            base.Render(inGameTime);

            spriteAnimatorRender.UpdateAnimator();

            RenderManager.GetInstance().totalBatch++;
            RenderManager.GetInstance().spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: RenderManager.GetInstance().screenScaleMatrix);
            //RenderManager.GetInstance().spriteBatch.Draw(sprites[_currentIndex], _transformComponent.position, null, Color.White, owner.transformComponent.rotation, Vector2.Zero, owner.transformComponent.scale, new SpriteEffects(), 0);
            RenderManager.GetInstance().spriteBatch.Draw(spriteAnimatorRender.GetCurrentSprite(), _transformComponent.position, Color.White);
            RenderManager.GetInstance().spriteBatch.End();
        }

        public override string ComponentToString()
        {
            return "Current Frame:" + spriteAnimatorRender.GetCurrentIndex();
        }
    }
}
