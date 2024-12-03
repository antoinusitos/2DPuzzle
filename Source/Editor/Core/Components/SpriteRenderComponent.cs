using ImGuiNET;
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
            sprite = ContentManager.GetInstance().GetSprite(spritePath);
        }

        public override void Render(GameTime inGameTime)
        {
            base.Render(inGameTime);

            if(sprite == null)
            {
                return;
            }

            RenderManager.GetInstance().totalBatch++;
            RenderManager.GetInstance().spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: RenderManager.GetInstance().screenScaleMatrix);
            //RenderManager.GetInstance().spriteBatch.Draw(sprite,  _transformComponent.position, Color.White, _transformComponent.rotation,, spriteEffects);
            RenderManager.GetInstance().spriteBatch.Draw(sprite, new Rectangle((int)_transformComponent.position.X, (int)_transformComponent.position.Y, sprite.Width, sprite.Height), null, Color.White, _transformComponent.rotation, new Vector2(0, 0), spriteEffects, 0f);
            RenderManager.GetInstance().spriteBatch.End();
        }

        public override string ComponentToString()
        {
            return "Unique ID:" + uniqueID + "\n" + "spritePath:" + spritePath + "\n" + "Layer:" + layer;
        }

        public override void EditorGUI()
        {
            ImGui.Text("Unique ID:" + uniqueID);
            ImGui.InputText("spritePath", ref spritePath, 32);
            if(ImGui.MenuItem("Load Sprite"))
            {
                LoadSprite();
            }
            ImGui.Text("Layer:" + layer);
        }

        public override SavedData GetSavedData()
        {
            SavedData savedData = new SavedData
            {
                savedString = new Dictionary<string, string>()
                {
                    { "Editor." + owner.name + ".spritePath", spritePath },
                },
                savedInt = new Dictionary<string, int>()
                {
                    { "Editor." + owner.name + ".Layer", layer },
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
            if (inSavedData.savedInt.ContainsKey("Editor." + owner.name + ".Layer"))
            {
                SwitchLayer(inSavedData.savedInt["Editor." + owner.name + ".Layer"]);
            }
        }

        public override void CloneComponent(ref EntityComponent inComponent)
        {
            SpriteRenderComponent spriteRenderComponent = inComponent as SpriteRenderComponent;
            spriteRenderComponent.spritePath = spritePath;
            spriteRenderComponent.layer = layer;
            spriteRenderComponent.SwitchLayer(layer);
            spriteRenderComponent.LoadSprite();
        }
    }
}
