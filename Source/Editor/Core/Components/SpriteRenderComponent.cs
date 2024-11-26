using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace _2DPuzzle
{
    public class SpriteRenderComponent : RenderComponent
    {
        public string spritePath = "";

        public SpriteRenderComponent() : base()
        {

        }

        public SpriteRenderComponent(Entity inOwner, string inSpritePath) : base(inOwner)
        {
            spritePath = inSpritePath;
            LoadSprite();
        }

        private void LoadSprite()
        {
            sprite = RenderManager.GetInstance().content.Load<Texture2D>(spritePath);
        }

        public override void Render(GameTime inGameTime)
        {
            base.Render(inGameTime);

            RenderManager.GetInstance().totalBatch++;
            RenderManager.GetInstance().spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: RenderManager.GetInstance().screenScaleMatrix);
            //RenderManager.GetInstance().spriteBatch.Draw(sprite,  _transformComponent.position, Color.White, _transformComponent.rotation,, spriteEffects);
            RenderManager.GetInstance().spriteBatch.Draw(sprite, new Rectangle((int)_transformComponent.position.X, (int)_transformComponent.position.Y, sprite.Width, sprite.Height), null, Color.White, _transformComponent.rotation, new Vector2(0, 0), spriteEffects, 0f);
            RenderManager.GetInstance().spriteBatch.End();
        }

        public override SavedData GetSavedData()
        {
            SavedData savedData = new SavedData
            {
                savedString = new Dictionary<string, string>()
                {
                    { "Editor." + owner.name + ".spritePath", spritePath },
                }
            };
            return savedData;
        }

        public override void LoadSavedData(SavedData inSavedData)
        {
            if (inSavedData.savedString.ContainsKey("Editor." + owner.name + ".spritePath"))
            {
                spritePath = inSavedData.savedString["Editor." + owner.name + ".spritePath"];
                LoadSprite();
            }
        }
    }
}
