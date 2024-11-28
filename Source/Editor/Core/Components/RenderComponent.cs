using Microsoft.Xna.Framework.Graphics;

namespace _2DPuzzle
{
    public class RenderComponent : EntityComponent
    {
        public Texture2D sprite = null;

        public int layer = 0;

        public bool flipHorizontally = false;

        public SpriteEffects spriteEffects;

        public RenderComponent()
        {
            canRender = true;
            RenderManager.GetInstance().RegisterRenderer(this);
        }

        public RenderComponent(Entity inOwner, bool inMustRegister = true) : base(inOwner)
        {
            canRender = true;
            if(inMustRegister)
            {
                RenderManager.GetInstance().RegisterRenderer(this);
            }
        }

        public void SwitchLayer(int inLayer)
        {
            layer = inLayer;
            RenderManager.GetInstance().SwitchLayer(layer, inLayer, this);
        }

        public void SetFlipHorizontal(bool inFlipHorizontally)
        {
            flipHorizontally = inFlipHorizontally;
            if(flipHorizontally)
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }
            else
            {
                spriteEffects = SpriteEffects.None;
            }
        }
    }
}
