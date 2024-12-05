using Microsoft.Xna.Framework;
using System;

namespace _2DPuzzle
{
    public class GizmoComponent : RenderComponent
    {
        private bool isMoving = false;
        private Vector2 offset = Vector2.Zero;

        private bool movingOnX = false;
        private bool movingOnY = false;

        private const float gridSize = 16;
        private const bool useGrid = true;

        public GizmoComponent(Entity inOwner) : base(inOwner)
        {
        }

        public override void Render(GameTime inGameTime)
        {
            base.Render(inGameTime);

            Vector2 mousePos = InputManager.GetInstance().GetMousePositionWorld();

            //BOTH
            if (mousePos.X > _transformComponent.position.X && mousePos.X <= _transformComponent.position.X + 5 &&
                mousePos.Y > _transformComponent.position.Y && mousePos.Y <= _transformComponent.position.Y + 5)
            {
                if (InputManager.GetInstance().IsMouseButtonPressed(0))
                {
                    if (!isMoving)
                    {
                        movingOnX = true;
                        movingOnY = true;
                        isMoving = true;
                        offset = _transformComponent.position - mousePos;
                    }
                }
            }

            //X
            else if (mousePos.X > _transformComponent.position.X && mousePos.X <= _transformComponent.position.X + 32 &&
                mousePos.Y > _transformComponent.position.Y && mousePos.Y <= _transformComponent.position.Y + 5)
            {
                if(InputManager.GetInstance().IsMouseButtonPressed(0))
                {
                    if(!isMoving)
                    {
                        movingOnX = true;
                        isMoving = true;
                        offset = _transformComponent.position - mousePos;
                    }
                }
            }
            //Y
            else if (mousePos.X > _transformComponent.position.X && mousePos.X <= _transformComponent.position.X + 5 &&
                mousePos.Y > _transformComponent.position.Y && mousePos.Y <= _transformComponent.position.Y + 32)
            {
                if (InputManager.GetInstance().IsMouseButtonPressed(0))
                {
                    if (!isMoving)
                    {
                        movingOnY = true;
                        isMoving = true;
                        offset = _transformComponent.position - mousePos;
                    }
                }
            }

            if (isMoving)
            {
                float finalX = movingOnX ? mousePos.X + offset.X : _transformComponent.position.X;
                float finalY = movingOnY ? mousePos.Y + offset.Y : _transformComponent.position.Y;

                if(useGrid)
                {
                    finalX /= gridSize;
                    finalX = MathF.Round(finalX);
                    finalX *= gridSize;

                    finalY /= gridSize;
                    finalY = MathF.Round(finalY);
                    finalY *= gridSize;
                }

                EditorManager.GetInstance().inspectedEntity.transformComponent.position = new Vector2(finalX, finalY);
            }

            if (isMoving && InputManager.GetInstance().IsMouseButtonReleased(0))
            {
                isMoving = false;
                movingOnX = false;
                movingOnY = false;
            }
        }
    }
}
