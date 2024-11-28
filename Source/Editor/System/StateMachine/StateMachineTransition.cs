using System.Collections.Generic;

namespace _2DPuzzle
{
    public enum Condition
    {
        EQUAL,
        SUP,
        INF,
        SUPEQUAL,
        INFEQUAL,
        DIFF
    }

    public class TransitionCondition
    {
        public string parameter = "";
        public Condition condition;
        public float value = 0f;
    }

    public class StateMachineTransition
    {
        public StateMachineState fromState = null;
        public StateMachineState toState = null;

        public TransitionCondition transitionCondition;

        public StateMachineComponent parentStateMachine = null;

        public uint uniqueID = 0;

        public bool EvaluateCondition(StateMachineComponent inParentStateMachine)
        {
            switch(transitionCondition.condition)
            {
                case Condition.EQUAL:
                    return inParentStateMachine.GetParameterValue(transitionCondition.parameter) == transitionCondition.value;
                case Condition.SUP:
                    return inParentStateMachine.GetParameterValue(transitionCondition.parameter) > transitionCondition.value;
                case Condition.INF:
                    return inParentStateMachine.GetParameterValue(transitionCondition.parameter) < transitionCondition.value;
                case Condition.SUPEQUAL:
                    return inParentStateMachine.GetParameterValue(transitionCondition.parameter) >= transitionCondition.value;
                case Condition.INFEQUAL:
                    return inParentStateMachine.GetParameterValue(transitionCondition.parameter) <= transitionCondition.value;
                case Condition.DIFF:
                    return inParentStateMachine.GetParameterValue(transitionCondition.parameter) != transitionCondition.value;
            }

            return false;
        }

        public virtual SavedData GetSavedData()
        {
            SavedData savedData = new SavedData();
            savedData.savedInt = new Dictionary<string, int>()
            {
                {"Editor." + parentStateMachine.owner.name + ".transition.condition." + uniqueID, (int)transitionCondition.condition }
            };
            savedData.savedString = new Dictionary<string, string>()
            {
                {"Editor." + parentStateMachine.owner.name + ".transition.parameter." + uniqueID, transitionCondition.parameter },
                {"Editor." + parentStateMachine.owner.name + ".transition.From." + uniqueID, fromState.uniqueID.ToString() },
                {"Editor." + parentStateMachine.owner.name + ".transition.To." + uniqueID, toState.uniqueID.ToString() },
            };
            savedData.savedFloat = new Dictionary<string, float>()
            {
                {"Editor." + parentStateMachine.owner.name + ".transition.value." + uniqueID, transitionCondition.value }
            };

            return savedData;
        }

        public virtual void LoadSavedData(SavedData inSavedData)
        {
            transitionCondition = new TransitionCondition();

            if (inSavedData.savedInt.ContainsKey("Editor." + parentStateMachine.owner.name + ".transition.condition." + uniqueID))
            {
                transitionCondition.condition = (Condition)inSavedData.savedInt["Editor." + parentStateMachine.owner.name + ".transition.condition." + uniqueID];
            }

            if (inSavedData.savedString.ContainsKey("Editor." + parentStateMachine.owner.name + ".transition.parameter." + uniqueID))
            {
                transitionCondition.parameter = inSavedData.savedString["Editor." + parentStateMachine.owner.name + ".transition.parameter." + uniqueID];
            }

            if (inSavedData.savedFloat.ContainsKey("Editor." + parentStateMachine.owner.name + ".transition.value." + uniqueID))
            {
                transitionCondition.value = inSavedData.savedFloat["Editor." + parentStateMachine.owner.name + ".transition.value." + uniqueID];
            }

            if (inSavedData.savedString.ContainsKey("Editor." + parentStateMachine.owner.name + ".transition.From." + uniqueID))
            {
                uint FromID = uint.Parse(inSavedData.savedString["Editor." + parentStateMachine.owner.name + ".transition.From." + uniqueID]);
                List<AnimationState> states = ((AnimatorComponent)parentStateMachine).allStates;
                for (int stateIndex = 0; stateIndex < states.Count; stateIndex++)
                {
                    if (states[stateIndex].uniqueID == FromID)
                    {
                        fromState = states[stateIndex];
                        break;
                    }
                }
            }
            if (inSavedData.savedString.ContainsKey("Editor." + parentStateMachine.owner.name + ".transition.To." + uniqueID))
            {
                uint ToID = uint.Parse(inSavedData.savedString["Editor." + parentStateMachine.owner.name + ".transition.To." + uniqueID]);
                List<AnimationState> states = ((AnimatorComponent)parentStateMachine).allStates;
                for (int stateIndex = 0; stateIndex < states.Count; stateIndex++)
                {
                    if (states[stateIndex].uniqueID == ToID)
                    {
                        toState = states[stateIndex];
                        break;
                    }
                }
            }
        }
    }
}
