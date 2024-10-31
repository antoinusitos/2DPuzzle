using Microsoft.Xna.Framework.Graphics;

namespace _2DPuzzle
{
    public class RenderComponent : EntityComponent
    {
        public Texture2D sprite = null;

        public int layer = 0;

        public RenderComponent(Entity inOwner) : base(inOwner)
        {
            canRender = true;
            RenderManager.GetInstance().RegisterRenderer(this);
        }

        public void SwitchLayer(int inLayer)
        {
            RenderManager.GetInstance().SwitchLayer(layer, inLayer, this);
        }
    }
}
