using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static System.Formats.Asn1.AsnWriter;
using System.Collections.Generic;

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

        public PhysicsComponent() : base()
        {
            CollisionManager.GetInstance().RegisterPhysicsComponent(this);
            canUpdate = true;
            RenderManager.GetInstance().RegisterRenderer(this);

            whiteRectangle = new Texture2D(RenderManager.GetInstance().graphicsDevice, 1, 1);
            whiteRectangle.SetData(new[] { Color.White });
        }

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

        public override string ComponentToString()
        {
            return "mass:" + mass + "\n" +
                    "velocity:" + velocity + "\n" +
                    "Use Gravity:" + useGravity;
        }

        public override SavedData GetSavedData()
        {
            SavedData savedData = new SavedData
            {
                savedFloat = new Dictionary<string, float>
                {
                    { "Editor." + owner.name + ".Mass", mass },
                },
                savedBool = new Dictionary<string, bool>
                {
                    { "Editor." + owner.name + ".UseGravity", useGravity },
                },
                savedInt = new Dictionary<string, int>
                {
                    { "Editor." + owner.name + ".CollisionType", (int)collisionType },
                }
            };
            return savedData;
        }

        public override void LoadSavedData(SavedData inSavedData)
        {
            if (inSavedData.savedFloat.ContainsKey("Editor." + owner.name + ".Mass"))
            {
                mass = inSavedData.savedFloat["Editor." + owner.name + ".Mass"];
            }

            if (inSavedData.savedBool.ContainsKey("Editor." + owner.name + ".UseGravity"))
            {
                useGravity = inSavedData.savedBool["Editor." + owner.name + ".UseGravity"];
            }

            if (inSavedData.savedInt.ContainsKey("Editor." + owner.name + ".CollisionType"))
            {
                collisionType = (CollisionType)inSavedData.savedInt["Editor." + owner.name + ".CollisionType"];
            }
        }
    }
}
