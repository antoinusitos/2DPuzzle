﻿using Microsoft.Xna.Framework;

namespace _2DPuzzle
{
    public class PlayerMovementComponent : EntityComponent
    {
        private const float speed = 100;

        public PlayerMovementComponent(Entity inOwner) : base(inOwner)
        {
            SetCanUpdate(true);
        }

        public override void Update(GameTime inGameTime)
        {
            base.Update(inGameTime);

            if (_transformComponent == null)
            {
                return;
            }

            float horizontal = InputManager.GetInstance().IsKeyDown("Right") ? 1 : 0;
            horizontal += InputManager.GetInstance().IsKeyDown("Left") ? -1 : 0;

            float vertical = InputManager.GetInstance().IsKeyDown("Up") ? -1 : 0;
            vertical += InputManager.GetInstance().IsKeyDown("Down") ? 1 : 0;

            Vector2 movement = new Vector2(horizontal, vertical);

            if (movement == Vector2.Zero)
            {
                return;
            }

            movement.Normalize();

            _transformComponent.position += UpdateManager.GetInstance().deltaTime * speed * movement;
        }
    }
}
