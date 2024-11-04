using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2DPuzzle
{
    public class TextRenderComponent : RenderComponent
    {
        protected SpriteFont _font = null;

        protected string text = "";

        protected Color textColor = Color.Black;

        public TextRenderComponent(Entity inOwner, string inFontPath) : base(inOwner)
        {
            _font = RenderManager.GetInstance().content.Load<SpriteFont>(inFontPath);
        }

        public override void Render(GameTime inGameTime)
        {
            base.Render(inGameTime);

            RenderManager.GetInstance().totalBatch++;
            RenderManager.GetInstance().spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            RenderManager.GetInstance().spriteBatch.DrawString(_font, text, owner.transformComponent.position, textColor);
            RenderManager.GetInstance().spriteBatch.End();
        }
    }
}
