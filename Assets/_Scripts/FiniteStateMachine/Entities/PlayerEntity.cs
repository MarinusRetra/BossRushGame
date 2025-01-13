using BossRush.FiniteStateMachine.Behaviors.MovementStates;
using UnityEngine;

namespace BossRush.FiniteStateMachine.Entities
{
    public class PlayerEntity : Entity
    {
        [Header("Movement Variables")]
        public float CurrentMoveSpeed = 10;
        public float JumpHeight = 10;
        public float CrouchingMoveSpeed = 10;
        public float WalkingMoveSpeed = 10;
        public float SprintMoveSpeed = 30;
        public float SlideMoveSpeed = 40;
        public float SlideJumpMoveSpeed = 50;

        public float sensivityY = 0.5f; // [TEMP] Will later be moved to a camera controller.
        public float yRotation;
        public Vector3 moveDirection;

        [Header("Movement States")]
        public IdleState IdleState;
        public WalkingState WalkingState;
        public RunningState RunningState;
        public JumpingState JumpingState;
        public FallingState FallingState;
        public CrouchingState CrouchingState;
        public SlidingState SlidingState;
        public UIState UIState;

        public override void Initialize()
        {
            base.Initialize();

            IdleState = new IdleState(Machine);
            WalkingState = new WalkingState(Machine);
            RunningState = new RunningState(Machine);
            JumpingState = new JumpingState(Machine);
            FallingState = new FallingState(Machine);
            CrouchingState = new CrouchingState(Machine);
            SlidingState = new SlidingState(Machine);
            UIState = new UIState(Machine);

            Machine.SetState(IdleState);
        }
    }
}
