using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2DPuzzle
{
    public class SpriteRenderComponent : RenderComponent
    {
        public SpriteRenderComponent(Entity inOwner, string inSpritePath) : base(inOwner)
        {
            sprite = RenderManager.GetInstance().content.Load<Texture2D>(inSpritePath);
        }

        public override void Render(GameTime inGameTime)
        {
            base.Render(inGameTime);

            RenderManager.GetInstance().totalBatch++;
            RenderManager.GetInstance().spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: RenderManager.GetInstance().screenScaleMatrix);
            RenderManager.GetInstance().spriteBatch.Draw(sprite, _transformComponent.position, Color.White);
            RenderManager.GetInstance().spriteBatch.End();
        }
    }
}
