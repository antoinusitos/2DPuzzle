using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace _2DPuzzle
{
    public class AnimatorComponent : StateMachineComponent
    {
        private AnimationState currentAnimationState = null;

        //Use this to save only
        public List<AnimationState> allStates = new List<AnimationState>();
        public List<StateMachineTransition> allTransitions = new List<StateMachineTransition>();

        public AnimatorComponent() : base()
        {
            canUpdate = true;
            canRender = true;
            RenderManager.GetInstance().RegisterRenderer(this);
        }

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

        public override SavedData GetSavedData()
        {
            SavedData toReturn = base.GetSavedData();
            if(toReturn.savedInt == null)
            {
                toReturn.savedInt = new Dictionary<string, int>();
            }
            if(toReturn.savedFloat == null)
            {
                toReturn.savedFloat = new Dictionary<string, float>();
            }

            toReturn.savedInt.Add("Editor." + owner.name + ".StatesNumber", allStates.Count);
            toReturn.savedInt.Add("Editor." + owner.name + ".TransitionsNumber", allTransitions.Count);
            for (int stateIndex = 0; stateIndex < allStates.Count; stateIndex++)
            {
                SavedData tempSavedData = allStates[stateIndex].GetSavedData();
                if(tempSavedData.savedFloat != null)
                {
                    for(KeyValuePair<string, float> singleState : )
                    for(int floatIndex = 0; floatIndex < tempSavedData.savedFloat.Count; floatIndex++)
                    {
                        toReturn.savedFloat.Add(tempSavedData.savedFloat[floatIndex], )
                    }
                }
            }
            return toReturn;
        }
    }
}
