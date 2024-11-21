using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2DPuzzle
{
    public class AnimatorComponent : StateMachineComponent
    {
        private AnimationState currentAnimationState = null;

        public AnimatorComponent(Entity inOwner) : base(inOwner)
        {
            canUpdate = true;
            canRender = true;
            RenderManager.GetInstance().RegisterRenderer(this);
        }

        public override void Render(GameTime inGameTime)
        {
            base.Render(inGameTime);

            RenderManager.GetInstance().totalBatch++;
            RenderManager.GetInstance().spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: RenderManager.GetInstance().screenScaleMatrix);
            //RenderManager.GetInstance().spriteBatch.Draw(sprites[_currentIndex], _transformComponent.position, null, Color.White, owner.transformComponent.rotation, Vector2.Zero, owner.transformComponent.scale, new SpriteEffects(), 0);
            RenderManager.GetInstance().spriteBatch.Draw(currentAnimationState.spriteAnimatorRender.GetCurrentSprite(), _transformComponent.position, Color.White);
            RenderManager.GetInstance().spriteBatch.End();
        }

        public override void ChangeState(StateMachineState toState)
        {
            currentState.OnExit();
            currentState = toState;
            currentAnimationState = (AnimationState)currentState;
            currentState.OnEnter();
        }

        public override void SetStartingState(StateMachineState inStartingState)
        {
            startingState = inStartingState;
            currentState = startingState;
            currentAnimationState = (AnimationState)currentState;
        }

        public override string ComponentToString()
        {
            return "Current Frame:" + currentAnimationState.spriteAnimatorRender.GetCurrentIndex();
        }
    }
}
