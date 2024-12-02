using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using ImGuiNET;

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

        private int _rectangeSizeX = 1;
        private int _rectangeSizeY = 1;

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
            rectangle.Width = _rectangeSizeX;
            rectangle.Height = _rectangeSizeY;

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
                    "Collision Type:" + collisionType + "\n" +
                    "Rectangle Pos:" + rectangle.Location.ToString() + "\n" +
                    "Rectangle Size:" + rectangle.Size.ToString();
        }

        public override void EditorGUI()
        {
            rectangle.X = (int)owner.transformComponent.position.X;
            rectangle.Y = (int)owner.transformComponent.position.Y;
            rectangle.Width = _rectangeSizeX;
            rectangle.Height = _rectangeSizeY;

            Debug.DrawLine(new Vector2(rectangle.X, rectangle.Y), new Vector2(rectangle.X + rectangle.Width, rectangle.Y), Color.Green);
            Debug.DrawLine(new Vector2(rectangle.X, rectangle.Y), new Vector2(rectangle.X, rectangle.Y + rectangle.Height), Color.Green);
            Debug.DrawLine(new Vector2(rectangle.X + rectangle.Width, rectangle.Y), new Vector2(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height), Color.Green);
            Debug.DrawLine(new Vector2(rectangle.X, rectangle.Y + rectangle.Height), new Vector2(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height), Color.Green);

            ImGui.Text("Unique ID:" + uniqueID);
            ImGui.InputFloat("mass", ref mass);
            ImGui.Text("velocity:" + velocity);
            ImGui.Checkbox("Use Gravity", ref useGravity);

            string[] items = { "STATIC", "DYNAMIC" };
            if(ImGui.BeginCombo("Collision Type", items[(int)collisionType]))
            {
                for (int n = 0; n < items.Length; n++)
                {
                    bool is_selected = (collisionType.ToString() == items[n]); // You can store your selection however you want, outside or inside your objects
                    if (ImGui.Selectable(items[n], is_selected))
                    {
                        collisionType = (CollisionType)n;
                    }
                    if (is_selected)
                        ImGui.SetItemDefaultFocus();   // You may set the initial focus when opening the combo (scrolling + for keyboard navigation support)
                }
                ImGui.EndCombo();
            }

            ImGui.Text("Rectangle Pos:" + rectangle.Location.ToString());
            ImGui.InputInt("Rectangle Size X" , ref _rectangeSizeX);
            ImGui.InputInt("Rectangle Size Y", ref _rectangeSizeY);
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
