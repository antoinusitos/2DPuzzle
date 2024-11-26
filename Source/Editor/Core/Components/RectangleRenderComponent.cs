using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace _2DPuzzle
{
    public class RectangleRenderComponent : RenderComponent
    {
        public Rectangle rectangle;
        public Texture2D whiteRectangle;
        public Color color = Color.White;

        public RectangleRenderComponent() : base()
        {
            whiteRectangle = new Texture2D(RenderManager.GetInstance().graphicsDevice, 1, 1);
            whiteRectangle.SetData(new[] { Color.White });
        }

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
            RenderManager.GetInstance().spriteBatch.Draw(whiteRectangle, rectangle, color * 0.5f);
            RenderManager.GetInstance().spriteBatch.End();
        }

        public override SavedData GetSavedData()
        {
            SavedData savedData = new SavedData
            {
                savedFloat = new Dictionary<string, float>
                {
                    { "Editor." + owner.name + ".Color.R", color.R },
                    { "Editor." + owner.name + ".Color.G", color.G },
                    { "Editor." + owner.name + ".Color.B", color.B },
                    { "Editor." + owner.name + ".Color.A", color.A },
                },
                savedInt = new Dictionary<string, int>
                {
                    { "Editor." + owner.name + ".Rectangle.X", rectangle.X },
                    { "Editor." + owner.name + ".Rectangle.Y", rectangle.Y },
                    { "Editor." + owner.name + ".Rectangle.Height", rectangle.Height },
                    { "Editor." + owner.name + ".Rectangle.Width", rectangle.Width },
                }
            };
            return savedData;
        }

        public override void LoadSavedData(SavedData inSavedData)
        {
            float r = 0;
            float g = 0;
            float b = 0;
            float a = 0;
            if (inSavedData.savedFloat.ContainsKey("Editor." + owner.name + ".Color.R"))
            {
                r = inSavedData.savedFloat["Editor." + owner.name + ".Color.R"];
            }
            if (inSavedData.savedFloat.ContainsKey("Editor." + owner.name + ".Color.G"))
            {
                g = inSavedData.savedFloat["Editor." + owner.name + ".Color.G"];
            }
            if (inSavedData.savedFloat.ContainsKey("Editor." + owner.name + ".Color.B"))
            {
                b = inSavedData.savedFloat["Editor." + owner.name + ".Color.B"];
            }
            if (inSavedData.savedFloat.ContainsKey("Editor." + owner.name + ".Color.A"))
            {
                a = inSavedData.savedFloat["Editor." + owner.name + ".Color.A"];
            }

            color = new Color(r, g, b, a);

            int x = 0;
            int y = 0;
            int h = 0;
            int w = 0;
            if (inSavedData.savedInt.ContainsKey("Editor." + owner.name + ".Rectangle.X"))
            {
                x = inSavedData.savedInt["Editor." + owner.name + ".Rectangle.X"];
            }
            if (inSavedData.savedInt.ContainsKey("Editor." + owner.name + ".Rectangle.Y"))
            {
                y = inSavedData.savedInt["Editor." + owner.name + ".Rectangle.Y"];
            }
            if (inSavedData.savedInt.ContainsKey("Editor." + owner.name + ".Rectangle.Height"))
            {
                h = inSavedData.savedInt["Editor." + owner.name + ".Rectangle.Height"];
            }
            if (inSavedData.savedInt.ContainsKey("Editor." + owner.name + ".Rectangle.Width"))
            {
                w = inSavedData.savedInt["Editor." + owner.name + ".Rectangle.Width"];
            }

            rectangle = new Rectangle(x, y, w, h);
        }
    }
}
