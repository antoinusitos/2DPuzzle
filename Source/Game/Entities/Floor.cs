using Microsoft.Xna.Framework;

namespace _2DPuzzle
{
    public class Floor : Entity
    {
        public Floor(bool inInitializeNewEntity = true) : base(inInitializeNewEntity)
        {
            name = "Floor";

            RectangleRenderComponent rectangleRenderComponent = new RectangleRenderComponent(this)
            {
                rectangle = new Rectangle(75, 300, 200, 50)
            };
            components.Add(rectangleRenderComponent);
            PhysicsComponent physicsComponent = new PhysicsComponent(this)
            {
                rectangle = new Rectangle(75, 300, 200, 50)
            };
            components.Add(physicsComponent);
        }
    }
}
