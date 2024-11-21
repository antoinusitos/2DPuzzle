using Microsoft.Xna.Framework;

namespace _2DPuzzle
{
    public class TransformComponent : EntityComponent
    {
        public Vector2 position { get; set; } = Vector2.Zero;
        public float rotation { get; set; } = 0;
        public Vector2 scale { get; set; } = Vector2.One;

        public TransformComponent(Entity inOwner) : base(inOwner)
        {

        }

        public void PrintPosition()
        {
            Debug.Log("X:" + position.X + " | " + "Y:" + position.Y, owner);
        }

        public override string ComponentToString()
        {
            return "Position:" + position + "\n" + 
                    "Rotation:" + rotation + "\n" + 
                    "Scale:" + scale;
        }

        public override string Save()
        {
            return "TransformComponent\n[\n{position:" + position + "}\n" +
                    "{rotation:" + rotation + "}\n" +
                    "{scale:" + scale + "}\n]";
        }
    }
}
