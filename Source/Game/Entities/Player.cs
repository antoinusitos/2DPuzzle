﻿using Microsoft.Xna.Framework;

namespace _2DPuzzle
{
    public class Player : Entity
    {
        public Player()
        {
            name = "Player";

            //components.Add(new SpriteRenderComponent(this, "Idle"));
            //components.Add(new SpriteAnimatorRenderComponent(this, "Running/Running_", 8, true));
            SetupAnimation();

            components.Add(new PlayerMovementComponent(this));

            PhysicsComponent physicsComponent = new PhysicsComponent(this)
            {
                rectangle = new Rectangle(0, 0, 15, 24)
            };
            physicsComponent.SetCollisionType(CollisionType.DYNAMIC);
            components.Add(physicsComponent);
        }

        private void SetupAnimation()
        {
            AnimatorComponent animatorComponent = new AnimatorComponent(this);
            components.Add(animatorComponent);
            animatorComponent.parameters.Add("Running", 0);

            AnimationState idleState = new AnimationState
            {
                spriteAnimatorRender = new SpriteAnimatorRender("Idle", 1, false),
                parentStateMachine = animatorComponent
            };
            AnimationState runningState = new AnimationState
            {
                spriteAnimatorRender = new SpriteAnimatorRender("Running/Running_", 8, true),
                parentStateMachine = animatorComponent
            };

            StateMachineTransition idleToRunTransition = new StateMachineTransition
            {
                fromState = idleState,
                toState = runningState
            };
            idleToRunTransition.transitionCondition += () => { return animatorComponent.GetParameterValue("Running") > 0; };
            idleState.transitions.Add(idleToRunTransition);

            StateMachineTransition runToIdleTransition = new StateMachineTransition
            {
                fromState = runningState,
                toState = idleState
            };
            runToIdleTransition.transitionCondition += () => { return animatorComponent.GetParameterValue("Running") <= 0; };
            runningState.transitions.Add(runToIdleTransition);

            animatorComponent.SetStartingState(idleState);
            animatorComponent.SetStartingState(idleState);
        }
    }
}
