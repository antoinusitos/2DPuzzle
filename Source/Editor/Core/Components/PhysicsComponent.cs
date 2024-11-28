using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static System.Formats.Asn1.AsnWriter;
using System.Collections.Generic;
using ImGuiNET;
using System.Runtime.Intrinsics.X86;

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
            return "Unique ID:" + uniqueID + "\n" +
                    "mass:" + mass + "\n" +
                    "velocity:" + velocity + "\n" +
                    "Use Gravity:" + useGravity + "\n" +
                    "Rectangle Pos:" + rectangle.Location.ToString() + "\n" +
                    "Rectangle Size:" + rectangle.Size.ToString();
        }

        public override void EditorGUI()
        {
            ImGui.Text("Unique ID:" + uniqueID);
            ImGui.Text("mass:" + mass);
            ImGui.Text("velocity:" + velocity);
            ImGui.Text("Use Gravity:" + useGravity);
            ImGui.Text("Rectangle Pos:" + rectangle.Location.ToString());
            ImGui.Text("Rectangle Size:" + rectangle.Size.ToString());
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
                    { "Editor." + owner.name + ".Rectange.Width", rectangle.Width },
                    { "Editor." + owner.name + ".Rectange.Height", rectangle.Height },
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

            int width = 0;
            if (inSavedData.savedInt.ContainsKey("Editor." + owner.name + ".Rectange.Width"))
            {
                width = inSavedData.savedInt["Editor." + owner.name + ".Rectange.Width"];
            }
            int height = 0;
            if (inSavedData.savedInt.ContainsKey("Editor." + owner.name + ".Rectange.Height"))
            {
                height = inSavedData.savedInt["Editor." + owner.name + ".Rectange.Height"];
            }
            rectangle = new Rectangle(0, 0, width, height);
        }
    }
}
