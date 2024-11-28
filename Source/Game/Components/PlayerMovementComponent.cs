using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace _2DPuzzle
{
    public class PlayerMovementComponent : EntityComponent
    {
        private float speed = 100;

        private PhysicsComponent physicsComponent = null;

        private bool isJumping = false;

        public PlayerMovementComponent() : base()
        {
            SetCanUpdate(true);
        }

        public PlayerMovementComponent(Entity inOwner) : base(inOwner)
        {
            SetCanUpdate(true);
        }

        public override void Update(GameTime inGameTime)
        {
            base.Update(inGameTime);

            if(physicsComponent == null)
            {
                physicsComponent =  owner.GetComponent<PhysicsComponent>();
            }

            if (_transformComponent == null)
            {
                return;
            }

            float horizontal = InputManager.GetInstance().IsKeyDown("Right") ? 1 : 0;
            horizontal += InputManager.GetInstance().IsKeyDown("Left") ? -1 : 0;

            if(horizontal != 0)
            {
                owner.GetComponent<AnimatorComponent>().UpdateParameter("Running", 1);
            }
            else
            {
                owner.GetComponent<AnimatorComponent>().UpdateParameter("Running", 0);
            }

            float vertical = InputManager.GetInstance().IsKeyDown("Up") ? -1 : 0;
            vertical += InputManager.GetInstance().IsKeyDown("Down") ? 1 : 0;

            Vector2 movement = new Vector2(horizontal, vertical);

            if (movement != Vector2.Zero)
            {
                movement.Normalize();
            }

            if (InputManager.GetInstance().IsKeyDown("Jump"))
            {
                isJumping = true;
                movement = -Vector2.UnitY * 5;
            }

            Vector2 finalMovement = WorldManager.gravity * physicsComponent.mass * UpdateManager.GetInstance().deltaTime + movement * speed * UpdateManager.GetInstance().deltaTime;

            physicsComponent.velocity += finalMovement;
        }

        public override string ComponentToString()
        {
            return "Unique ID:" + uniqueID + "\n" + 
                    "isJumping:" + isJumping + "\n" +
                    "speed:" + speed;
        }

        public override SavedData GetSavedData()
        {
            SavedData savedData = new SavedData
            {
                savedFloat = new Dictionary<string, float>()
                {
                    { "Editor." + owner.name + ".Speed", speed },
                }
            };
            return savedData;
        }

        public override void LoadSavedData(SavedData inSavedData)
        {
            if (inSavedData.savedFloat.ContainsKey("Editor." + owner.name + ".Speed"))
            {
                speed = inSavedData.savedFloat["Editor." + owner.name + ".Speed"];
            }
        }
    }
}
