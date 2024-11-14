using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2DPuzzle
{
    public enum CollisionType
    {
        STATIC,
        DYNAMIC,
    };

    public class PhysicsComponent : EntityComponent
    {
        public bool useGravity = true;

        public Rectangle rectangle;

        public CollisionType collisionType = CollisionType.STATIC;

        public Vector2 velocity = Vector2.Zero;

        public Texture2D whiteRectangle;

        public float mass = 1.0f;

        public PhysicsComponent(Entity inOwner) : base(inOwner)
        {
            CollisionManager.GetInstance().RegisterPhysicsComponent(this);
            canUpdate = true;
            RenderManager.GetInstance().RegisterRenderer(this);

            whiteRectangle = new Texture2D(RenderManager.GetInstance().graphicsDevice, 1, 1);
            whiteRectangle.SetData(new[] { Color.White });
        }

        public void SetCollisionType(CollisionType inCollisionType)
        {
            collisionType = inCollisionType;
        }

        public override void Update(GameTime inGameTime)
        {
            base.Update(inGameTime);

            rectangle.X = (int)owner.transformComponent.position.X;
            rectangle.Y = (int)owner.transformComponent.position.Y;

            Debug.DrawLine(new Vector2(rectangle.X, rectangle.Y), new Vector2(rectangle.X + rectangle.Width, rectangle.Y), Color.Green);
            Debug.DrawLine(new Vector2(rectangle.X, rectangle.Y), new Vector2(rectangle.X, rectangle.Y + rectangle.Height), Color.Green);
            Debug.DrawLine(new Vector2(rectangle.X + rectangle.Width, rectangle.Y), new Vector2(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height), Color.Green);
            Debug.DrawLine(new Vector2(rectangle.X, rectangle.Y + rectangle.Height), new Vector2(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height), Color.Green);
        }
    }
}
