using Newtonsoft.Json;

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

        public virtual SavedData GetSavedData()
        {
            return new SavedData();
        }

        public virtual void LoadSavedData(SavedData inSavedData)
        {

        }
    }
}
