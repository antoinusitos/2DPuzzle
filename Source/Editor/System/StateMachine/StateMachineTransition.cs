using Newtonsoft.Json;

namespace _2DPuzzle
{
    public delegate bool TransitionCondition();

    public class StateMachineTransition
    {
        public StateMachineState fromState = null;
        public StateMachineState toState = null;

        [JsonIgnore]
        public TransitionCondition transitionCondition;
    }
}
