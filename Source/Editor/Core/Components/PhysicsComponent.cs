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

        public PhysicsComponent(Entity inOwner) : base(inOwner)
        {
            CollisionManager.GetInstance().RegisterPhysicsComponent(this);
            canUpdate = true;
            /*canRender = true;
            RenderManager.GetInstance().RegisterRenderer(this);*/

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
        }

        public override void Render(GameTime inGameTime)
        {
            base.Render(inGameTime);

            RenderManager.GetInstance().totalBatch++;
            RenderManager.GetInstance().spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: RenderManager.GetInstance().screenScaleMatrix);
            RenderManager.GetInstance().spriteBatch.Draw(whiteRectangle, rectangle, Color.White);
            RenderManager.GetInstance().spriteBatch.End();
        }
    }
}
