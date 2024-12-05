using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace _2DPuzzle
{
    public class DebugMousePositionComponent : TextRenderComponent
    {
        public DebugMousePositionComponent(Entity inOwner, string inFontPath) : base(inOwner, inFontPath)
        {
            canUpdate = true;
        }

        public override void Update(GameTime inGameTime)
        {
            base.Update(inGameTime);

            Matrix inverseTransform = Matrix.Invert(RenderManager.GetInstance().screenScaleMatrix);
            Vector2 mouseInWorld = Vector2.Transform(new Vector2(Mouse.GetState().X, Mouse.GetState().Y), inverseTransform);

            text = "X:" + Mouse.GetState().X + " | Y:" + Mouse.GetState().Y + "     " + "(X World :" + mouseInWorld.X + " | Y World:" + mouseInWorld.Y + ")";
        }
    }
}
