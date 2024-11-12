using System.Collections.Generic;

namespace _2DPuzzle
{
    public class StateMachineState
    {
        public StateMachineComponent parentStateMachine = null;

        public List<StateMachineTransition> transitions = new List<StateMachineTransition>();

        public virtual void OnEnter()
        {

        }
        public virtual void OnUpdate()
        {
            for(int conditionIndex = 0; conditionIndex < transitions.Count; conditionIndex++)
            {
                if(transitions[conditionIndex].transitionCondition == null)
                {
                    Debug.LogWarning("A state machine transition has no condition");
                    continue;
                }

                if (transitions[conditionIndex].transitionCondition.Invoke())
                {
                    parentStateMachine.ChangeState(transitions[conditionIndex].toState);
                    return;
                }
            }
        }

        public virtual void OnExit()
        {

        }
    }
}
