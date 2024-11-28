using System.Collections.Generic;

namespace _2DPuzzle
{
    public class StateMachineState
    {
        public StateMachineComponent parentStateMachine = null;

        public List<StateMachineTransition> transitions = new List<StateMachineTransition>();

        public uint uniqueID = 0;

        public virtual void OnEnter()
        {

        }

        public virtual void Start()
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

                if (transitions[conditionIndex].EvaluateCondition(parentStateMachine))
                {
                    parentStateMachine.ChangeState(transitions[conditionIndex].toState);
                    return;
                }
            }
        }

        public virtual void OnExit()
        {

        }

        public virtual SavedData GetSavedData()
        {
            return new SavedData();
        }

        public virtual void LoadSavedData(SavedData inSavedData)
        {

        }
    }
}
