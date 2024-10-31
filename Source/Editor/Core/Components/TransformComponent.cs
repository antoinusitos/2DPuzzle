using Microsoft.Xna.Framework;

namespace _2DPuzzle
{
    public class TransformComponent : EntityComponent
    {
        public Vector2 position = Vector2.Zero;
        public float rotation = 0;
        public Vector2 scale = Vector2.One;

        public TransformComponent(Entity inOwner) : base(inOwner)
        {

        }

        public void PrintPosition()
        {
            Debug.Log("X:" + position.X + " | " + "Y:" + position.Y, owner);
        }
    }
}
