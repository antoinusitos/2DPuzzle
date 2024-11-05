using Microsoft.Xna.Framework;

namespace _2DPuzzle
{
    public class Player : Entity
    {
        public Player()
        {
            name = "Player";

            components.Add(new SpriteRenderComponent(this, "Idle"));
            //components.Add(new SpriteAnimatorRenderComponent(this, "Running/Running_", 8, true));
            components.Add(new PlayerMovementComponent(this));
            PhysicsComponent physicsComponent = new PhysicsComponent(this)
            {
                rectangle = new Rectangle(0, 0, 15, 24)
            };
            physicsComponent.SetCollisionType(CollisionType.DYNAMIC);
            components.Add(physicsComponent);
        }
    }
}
