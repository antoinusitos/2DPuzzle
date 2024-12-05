using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace _2DPuzzle
{
    public class DebugBackgroundComponent : RenderComponent
    {
        private Texture2D _whiteRectangle;

        public DebugBackgroundComponent(Entity inOwner) : base(inOwner)
        {
            _whiteRectangle = new Texture2D(RenderManager.GetInstance().graphicsDevice, 1, 1);
            _whiteRectangle.SetData(new[] { Color.White });
            SwitchLayer(-1);
        }

        public override void Render(GameTime inGameTime)
        {
            base.Render(inGameTime);

            int numberW = 8;
            int numberH = 4;

            int sizeW = 320 / numberW;
            int sizeH = 160 / numberH;

            RenderManager.GetInstance().totalBatch++;
            RenderManager.GetInstance().spriteBatch.Begin();

            Color c = Color.White;
            int line = 0;
            for (int j = 0; j < numberH; j++)
            {
                for (int i = 0; i < numberW; i++)
                {
                    if ((i + line) % 2 == 0)
                    {
                        c = Color.White;
                    }
                    else
                    {
                        c = Color.Gray;
                    }
                    RenderManager.GetInstance().spriteBatch.Draw(_whiteRectangle, new Rectangle(i * sizeW, j * sizeH + 10, sizeW, sizeH), c);
                }
                line++;
            }

            RenderManager.GetInstance().spriteBatch.End();
        }
    }
}
