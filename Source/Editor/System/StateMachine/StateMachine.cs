using System.Collections.Generic;

namespace _2DPuzzle
{
    public class StateMachine
    {
        public Dictionary<string, float> parameters = new Dictionary<string, float>();

        public StateMachineState currentState = null;
        public StateMachineState startingState = null;

        public void UpdateParameter(string inKey, float inValue)
        {
            if(!parameters.ContainsKey(inKey))
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

        public void SetStartingState(StateMachineState inStartingState)
        {
            startingState = inStartingState;
            currentState = startingState;
        }

        public void ChangeState(StateMachineState toState)
        {
            currentState.OnExit();
            currentState = toState;
            currentState.OnEnter();
        }

        public void OnUpdate()
        {
            currentState.OnUpdate();
        }
    }
}
