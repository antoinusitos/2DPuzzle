using Microsoft.Xna.Framework;

namespace _2DPuzzle
{
    public class DebugRectangle : Entity
    {
        public DebugRectangle(Vector2 inPos, Vector2 inSize,bool inInitializeNewEntity = true) : base(inInitializeNewEntity)
        {
            name = "DebugRectangle";

            transformComponent.position = inPos;
            RectangleRenderComponent rectangleRenderComponent = new RectangleRenderComponent(this)
            {
                rectangle = new Rectangle((int)inPos.X, (int)inPos.Y, (int)inSize.X, (int)inSize.Y),
                color = Color.Green,
                uniqueID = EditorManager.GetInstance().GetNewUniqueID()
            };
            components.Add(rectangleRenderComponent);
        }
    }
}
