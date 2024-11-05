using Microsoft.Xna.Framework.Graphics;

namespace _2DPuzzle
{
    public class RenderComponent : EntityComponent
    {
        public Texture2D sprite = null;

        public int layer = 0;

        public bool flipHorizontally = false;

        public SpriteEffects spriteEffects; 

        public RenderComponent(Entity inOwner) : base(inOwner)
        {
            canRender = true;
            RenderManager.GetInstance().RegisterRenderer(this);
        }

        public void SwitchLayer(int inLayer)
        {
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
