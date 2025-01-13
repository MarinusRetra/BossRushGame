using UnityEngine;
using BossRush.FiniteStateMachine.Behaviors.MovementStates;


namespace BossRush.FiniteStateMachine.Entities
{
    public class PlayerEntity : Entity
    {
        [Header("GeneralStuff")]
        public Canvas UICanvas;

        [Header("Movement Variables")]
        public float CurrentMoveSpeed = 10;
        public float JumpHeight = 10;
        public float CrouchingMoveSpeed = 5;
        public float WalkingMoveSpeed = 10;
        public float SprintMoveSpeed = 30;
        public float SlideMoveSpeed = 40;
        public float SlideJumpMoveSpeed = 50;
        public float groundCheckDistance = 1.2f;
        public LayerMask groundLayer;          
        public bool isGrounded;

        [HideInInspector] public float xInput, yInput;
        [HideInInspector] public float sensivityY = 0.5f; // [TEMP] Will later be moved to a camera controller.
        [HideInInspector] public float yRotation;
        [HideInInspector] public Vector3 moveDirection;

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
        // A lot of states have behaviours that overlap, i made these function just so i dont have to write the same function with the same logic multiple times in multiple scripts.
        /// <summary>
        /// Sets the current state to the pause state
        /// </summary>
        public void BasePause()
        {
            Machine.SetState(UIState);
        }
        /// <summary>
        /// Sets yRotation to xInput from the mouse or a joystick
        /// </summary>
        /// <param name="vector"></param>
        public void BaseLook(Vector2 vector)
        {
           yRotation += vector.x * sensivityY;

           transform.rotation = Quaternion.Euler(0, yRotation, 0);
        }
        /// <summary>
        /// SetState to RunningState
        /// </summary>
        public void BaseSprint()
        {
            Machine.SetState(RunningState);
        }
        /// <summary>
        /// SetState to JumpingState
        /// </summary>
        public void BaseJump()
        {
            Machine.SetState(JumpingState);
        }
        /// <summary>
        /// SetState to CrouchingState
        /// </summary>
        public void BaseCrouch()
        {
            Machine.SetState(CrouchingState); 
        }

        public void BaseBasicAttack()
        {
            Debug.Log("Basic Bonk");
        }

        public void BaseTertiary()
        {
            Debug.Log("Third Bonk");
        }

        public void BaseSecondary()
        {
            Debug.Log("Second Bonk");
        }

        public void BasePrimary()
        {
            Debug.Log("First Bonk");
        }
        /// <summary>
        /// Sets the playerEntity.xInput and yInput to the input axis from WASD or joystick input
        /// </summary>
        /// <param name="vector"></param>
        public void BaseMove(Vector2 vector)
        {
            xInput = vector.x;
            yInput = vector.y;
        }



    }
}
