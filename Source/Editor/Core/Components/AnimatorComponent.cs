using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2DPuzzle
{
    public class AnimatorComponent : EntityComponent
    {
        public StateMachine stateMachine = null;

        public AnimatorComponent(Entity inOwner) : base(inOwner)
        {
            stateMachine = new StateMachine();
            canUpdate = true;
            canRender = true;
            RenderManager.GetInstance().RegisterRenderer(this);
        }

        public override void Update(GameTime inGameTime)
        {
            base.Update(inGameTime);

            stateMachine.OnUpdate();
        }

        public override void Render(GameTime inGameTime)
        {
            base.Render(inGameTime);

            RenderManager.GetInstance().totalBatch++;
            RenderManager.GetInstance().spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: RenderManager.GetInstance().screenScaleMatrix);
            //RenderManager.GetInstance().spriteBatch.Draw(sprites[_currentIndex], _transformComponent.position, null, Color.White, owner.transformComponent.rotation, Vector2.Zero, owner.transformComponent.scale, new SpriteEffects(), 0);
            AnimationState animationState = (AnimationState)stateMachine.currentState;
            RenderManager.GetInstance().spriteBatch.Draw(animationState.spriteAnimatorRender.GetCurrentSprite(), _transformComponent.position, Color.White);
            RenderManager.GetInstance().spriteBatch.End();
        }
    }
}
