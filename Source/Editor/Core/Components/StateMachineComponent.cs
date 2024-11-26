using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace _2DPuzzle
{
    public class StateMachineComponent : EntityComponent
    {
        public Dictionary<string, float> parameters = new Dictionary<string, float>();

        public StateMachineState currentState = null;
        public StateMachineState startingState = null;

        public StateMachineComponent() : base()
        {

        }

        public StateMachineComponent(Entity inOwner) : base(inOwner)
        {

        }

        public void UpdateParameter(string inKey, float inValue)
        {
            if (!parameters.ContainsKey(inKey))
            {
                return;
            }

            parameters[inKey] = inValue;
        }

        public float GetParameterValue(string inKey)
        {
            if (!parameters.ContainsKey(inKey))
            {
                return -1;
            }

            return parameters[inKey];
        }

        public virtual void SetStartingState(StateMachineState inStartingState)
        {
            startingState = inStartingState;
            currentState = startingState;
        }

        public virtual void ChangeState(StateMachineState toState)
        {
            currentState.OnExit();
            currentState = toState;
            currentState.OnEnter();
        }

        public override void Update(GameTime inGameTime)
        {
            base.Update(inGameTime);

            currentState.OnUpdate();
        }
    }
}
