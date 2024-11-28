using Microsoft.Xna.Framework;

namespace _2DPuzzle
{
    public class Player : Entity
    {
        public Player(bool inInitializeNewEntity = true) : base(inInitializeNewEntity)
        {
            name = "Player";

            SetupAnimation();

            components.Add(new PlayerMovementComponent(this) { uniqueID = EditorManager.GetInstance().GetNewUniqueID()});

            PhysicsComponent physicsComponent = new PhysicsComponent(this)
            {
                rectangle = new Rectangle(0, 0, 15, 24),
                mass = 10,
                uniqueID = EditorManager.GetInstance().GetNewUniqueID()
            };
            physicsComponent.SetCollisionType(CollisionType.DYNAMIC);
            components.Add(physicsComponent);
        }

        private void SetupAnimation()
        {
            AnimatorComponent animatorComponent = new AnimatorComponent(this)
            {
                uniqueID = EditorManager.GetInstance().GetNewUniqueID(),
                layer = 1
            };
            components.Add(animatorComponent);
            RenderManager.GetInstance().SwitchLayer(0, 1, animatorComponent);
            animatorComponent.parameters.Add("Running", 0);

            AnimationState idleState = new AnimationState
            {
                parentStateMachine = animatorComponent,
                parentAnimatorComponent = animatorComponent,
                uniqueID = EditorManager.GetInstance().GetNewUniqueID(),
                animationStateName = "Idle"
            };
            idleState.SetAnimation("Idle", 1, false, false);
            AnimationState runningState = new AnimationState
            {
                parentStateMachine = animatorComponent,
                parentAnimatorComponent = animatorComponent,
                uniqueID = EditorManager.GetInstance().GetNewUniqueID(),
                animationStateName = "Running"
            };
            runningState.SetAnimation("Running/Running_", 8, true, false);

            animatorComponent.allStates.Add(idleState);
            animatorComponent.allStates.Add(runningState);

            StateMachineTransition idleToRunTransition = new StateMachineTransition
            {
                fromState = idleState,
                toState = runningState,
                parentStateMachine = animatorComponent,
                uniqueID = EditorManager.GetInstance().GetNewUniqueID()
            };
            idleToRunTransition.transitionCondition = new TransitionCondition() { parameter = "Running", condition = Condition.SUP, value = 0 };
            idleState.transitions.Add(idleToRunTransition);

            StateMachineTransition runToIdleTransition = new StateMachineTransition
            {
                fromState = runningState,
                toState = idleState,
                parentStateMachine = animatorComponent,
                uniqueID = EditorManager.GetInstance().GetNewUniqueID()
            };
            runToIdleTransition.transitionCondition = new TransitionCondition() { parameter = "Running", condition = Condition.INFEQUAL, value = 0 };
            runningState.transitions.Add(runToIdleTransition);

            animatorComponent.allTransitions.Add(idleToRunTransition);
            animatorComponent.allTransitions.Add(runToIdleTransition);
            animatorComponent.SetStartingState(idleState);
        }
    }
}
