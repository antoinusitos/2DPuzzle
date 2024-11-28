using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace _2DPuzzle
{
    public class SpriteAnimatorRenderComponent : RenderComponent
    {
        public SpriteAnimatorRender spriteAnimatorRender = null;

        private bool loop = false;
        private int frameNumber = 0;
        private string spritePath = "";

        public SpriteAnimatorRenderComponent(Entity inOwner, string inSpritePath, int inFrameNumber, bool inLoop, bool inMustRegister = true) : base(inOwner, inMustRegister)
        {
            loop = inLoop;
            frameNumber = inFrameNumber;
            spritePath = inSpritePath;
            LoadSpriteAnimatorRender();
        }

        public override void Render(GameTime inGameTime)
        {
            base.Render(inGameTime);

            spriteAnimatorRender.UpdateAnimator();

            Debug.Log("Rendering : " + spritePath);

            RenderManager.GetInstance().totalBatch++;
            RenderManager.GetInstance().spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: RenderManager.GetInstance().screenScaleMatrix);
            //RenderManager.GetInstance().spriteBatch.Draw(sprites[_currentIndex], _transformComponent.position, null, Color.White, owner.transformComponent.rotation, Vector2.Zero, owner.transformComponent.scale, new SpriteEffects(), 0);
            RenderManager.GetInstance().spriteBatch.Draw(spriteAnimatorRender.GetCurrentSprite(), _transformComponent.position, Color.White);
            RenderManager.GetInstance().spriteBatch.End();
        }

        private void LoadSpriteAnimatorRender()
        {
            spriteAnimatorRender = new SpriteAnimatorRender(spritePath, frameNumber, loop);
        }

        public override string ComponentToString()
        {
            return "Unique ID:" + uniqueID + "\n" + "Current Frame:" + spriteAnimatorRender.GetCurrentIndex() + "\n" + "Layer:" + layer;
        }

        public override SavedData GetSavedData()
        {
            SavedData savedData = new SavedData
            {
                savedString = new Dictionary<string, string>
                {
                    { "Editor." + owner.name + ".SpritePath", spritePath },
                },
                savedBool = new Dictionary<string, bool>
                {
                    { "Editor." + owner.name + ".Loop", loop },
                },
                savedInt = new Dictionary<string, int>
                {
                    { "Editor." + owner.name + ".FrameNumber", frameNumber },
                    { "Editor." + owner.name + ".Layer", layer },
                }
            };
            return savedData;
        }

        public override void LoadSavedData(SavedData inSavedData)
        {
            if (inSavedData.savedString.ContainsKey("Editor." + owner.name + ".SpritePath"))
            {
                spritePath = inSavedData.savedString["Editor." + owner.name + ".SpritePath"];
            }

            if (inSavedData.savedBool.ContainsKey("Editor." + owner.name + ".Loop"))
            {
                loop = inSavedData.savedBool["Editor." + owner.name + ".Loop"];
            }

            if (inSavedData.savedInt.ContainsKey("Editor." + owner.name + ".FrameNumber"))
            {
                frameNumber = inSavedData.savedInt["Editor." + owner.name + ".FrameNumber"];
            }
            if (inSavedData.savedInt.ContainsKey("Editor." + owner.name + ".Layer"))
            {
                SwitchLayer(inSavedData.savedInt["Editor." + owner.name + ".Layer"]);
            }

            LoadSpriteAnimatorRender();
        }
    }
}
