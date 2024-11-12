using Microsoft.Xna.Framework;

namespace _2DPuzzle
{
    public class PlayerMovementComponent : EntityComponent
    {
        private const float speed = 100;

        private PhysicsComponent physicsComponent = null;

        private bool isJumping = false;

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
                physicsComponent.mass = 1;
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
                movement = -Vector2.UnitY * 10;
            }

            Vector2 finalMovement = WorldManager.gravity * physicsComponent.mass * UpdateManager.GetInstance().deltaTime + movement * speed * UpdateManager.GetInstance().deltaTime;

            physicsComponent.velocity += finalMovement;

            /*_transformComponent.position += UpdateManager.GetInstance().deltaTime * speed * finalMovement;
            
            if(_transformComponent.position.Y >= 300)
            {
                _transformComponent.position.Y = 300;
            }*/
        }
    }
}
