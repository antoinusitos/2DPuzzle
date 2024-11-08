﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2DPuzzle
{
    public class RectangleRenderComponent : RenderComponent
    {
        public Rectangle rectangle;
        public Texture2D whiteRectangle;

        public RectangleRenderComponent(Entity inOwner) : base(inOwner)
        {
            whiteRectangle = new Texture2D(RenderManager.GetInstance().graphicsDevice, 1, 1);
            whiteRectangle.SetData(new[] { Color.White });
        }

        public override void Render(GameTime inGameTime)
        {
            base.Render(inGameTime);

            RenderManager.GetInstance().totalBatch++;
            RenderManager.GetInstance().spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: RenderManager.GetInstance().screenScaleMatrix);
            RenderManager.GetInstance().spriteBatch.Draw(whiteRectangle, rectangle, Color.White);
            RenderManager.GetInstance().spriteBatch.End();
        }
    }
}
