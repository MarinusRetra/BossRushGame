using System;
using UnityEngine;
using BossRush.FiniteStateMachine.Behaviors.MovementStates;
using System.Collections;
using BossRush.Classes;
using BossRush.Classes.Abilities;
using BossRush.Managers;
using Unity.Netcode;
using UnityEngine.Playables;


namespace BossRush.FiniteStateMachine.Entities
{
    public class PlayerEntity : Entity
    {
        /// <summary>
        /// These will either be moved or removed later.
        /// </summary>
        [Header("Temp")]
        public float sensivityY = 0.5f; // [TEMP] Will later be moved to a camera controller..
        public MeshRenderer playerRenderer;// [TEMP] Just so i can see states better ingame.


        [Header("GeneralStuff")]
        public Canvas UICanvas;
        /// <summary>
        /// These are used as player movement variables.
        /// </summary>
        [Header("Movement Variables")]
        public float JumpHeight = 10;
        public float CrouchingMoveSpeed = 5;
        public float WalkingMoveSpeed = 10;
        public float SprintMoveSpeed = 30;
        public float SlideMoveSpeed = 40;
        public float SlideJumpMoveSpeed = 50;
        public float FallMultiplier = -2.5f;
        public float JumpBufferTimer = 0.3f;
        public float CoyoteTimeTimer = 0.3f;

        /// <summary>
        /// These values generally do not need to be assigned and are just for seeing if everything is working corrrectly.
        /// </summary>
        [Header("Debug")]
        public bool CanSlide = true;
        public float CurrentMoveSpeed = 10;
        public float JumpBufferCounter = 0f;
        public float CoyoteTimeCounter = 0f;
        public float xInput, yInput;
        public float yRotation;
        public Vector3 moveDirection;
        public float groundCheckDistance = 1;
        public bool TrueIsGrounded; // This is true or falls whether coyote time is higher than 0 or not.
        public bool IsGrounded;
        public LayerMask groundLayer;

        [Header("Movement States")]
        public IdleState IdleState;
        public WalkingState WalkingState;
        public RunningState RunningState;
        public JumpingState JumpingState;
        public FallingState FallingState;
        public CrouchingState CrouchingState;
        public SlidingState SlidingState;
        public UIState UIState;

        [Header("PlayableClasses variables")] 
        [SerializeField] private PlayableClass _currentClass;
        public EntityAbilityData EntityAbility;
        [SerializeField] private bool _canAttack = true;

        public override void Initialize()
        {
            base.Initialize();

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            EntityAbility = new EntityAbilityData(BlackboardRef.Animator, BlackboardRef.Rigidbody);

            _currentClass = ClassManager.Instance.ChangeClass("BERSERKER");

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

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            JumpBufferCounter -= Time.deltaTime;
            CoyoteTimeCounter -= Time.deltaTime;
            
            TrueIsGrounded = Physics.CheckBox(Transform.position + Vector3.down * groundCheckDistance, new Vector3(0.5f, 0.1f, 0.5f) / 2, Quaternion.identity, groundLayer); // Ground Check
            
            if (TrueIsGrounded)
            { 
                CoyoteTimeCounter = CoyoteTimeTimer;
            }

            IsGrounded = CoyoteTimeCounter > 0;

            _currentClass.PassiveAbility.Do(ref EntityAbility);

            if (EntityAbility.HasTakenDmg) EntityAbility.HasTakenDmg = false;
        }


        // A lot of states have behaviours that overlap, this so you can quickly see what function of what state don't change the default behaviour.

        /// <summary>
        /// Sets the current state to the pause state.
        /// </summary>
        public void BasePause()
        {
            Machine.SetState(UIState);
        }

        /// <summary>
        /// Sets yRotation to xInput from the mouse or a joystick.
        /// </summary>
        /// <param name="vector"></param>
        public void BaseLook(Vector2 vector)
        {
           yRotation += vector.x * sensivityY;

           transform.rotation = Quaternion.Euler(0, yRotation, 0);
        }

        /// <summary>
        /// SetState to RunningState.
        /// </summary>
        public void BaseSprint()
        {
            Machine.SetState(RunningState);
        }

        /// <summary>
        /// SetState to JumpingState.
        /// </summary>
        public void BaseJump()
        {
            Machine.SetState(JumpingState);
        }

        /// <summary>
        /// SetState to CrouchingState.
        /// </summary>
        public void BaseCrouch()
        {
            Machine.SetState(CrouchingState); 
        }
        /// <summary>
        /// BasicAttack.
        /// </summary>
        public void BaseBasicAttack()
        {
            if (!_canAttack) return;
            _canAttack = false;

            StartCoroutine(_currentClass.PrimaryAbility.Do(EntityAbility, _currentClass.Data.PrimaryData,
                _currentClass.Data.BaseDmg[_currentClass.Level], transform,
                returnValue => { _canAttack = returnValue; }));
            Debug.Log("Basic Bonk");
        }
        /// <summary>
        /// Third Ability.
        /// </summary>
        public void BaseTertiary()
        {
            if (!_canAttack) return;
            _canAttack = false;

            StartCoroutine(_currentClass.QuaternaryAbility.Do(EntityAbility, _currentClass.Data.QuaternaryData,
                _currentClass.Data.BaseDmg[_currentClass.Level], transform,
                returnValue => { _canAttack = returnValue; }));
            Debug.Log("Third Bonk");
        }
        /// <summary>
        /// Second Ability.
        /// </summary>
        public void BaseSecondary()
        {
            if (!_canAttack) return;
            _canAttack = false;

            StartCoroutine(_currentClass.TertiaryAbility.Do(EntityAbility, _currentClass.Data.TertiaryData,
                _currentClass.Data.BaseDmg[_currentClass.Level], transform,
                returnValue => { _canAttack = returnValue; }));
            Debug.Log("Second Bonk");
        }
        /// <summary>
        /// First Ability.
        /// </summary>
        public void BasePrimary()
        {
            if (!_canAttack) return;
            _canAttack = false;

            StartCoroutine(_currentClass.SecondaryAbility.Do(EntityAbility, _currentClass.Data.SecondaryData,
                _currentClass.Data.BaseDmg[_currentClass.Level], transform,
                returnValue => { _canAttack = returnValue; }));
            Debug.Log("First Bonk");
        }
        /// <summary>
        /// Sets the playerEntity.xInput and yInput to the input axis from WASD or joystick input.
        /// </summary>
        /// <param name="vector"></param>
        public void BaseMove(Vector2 vector)
        {
            xInput = vector.x;
            yInput = vector.y;
        }
        /// <summary>
        /// This is the function called from SlidingState and ends sliding after .6 seconds.
        /// </summary>
        public void SlideJumpCoroutine() // Dont call this outside of sliding state.
        {
            StartCoroutine(SlideJumpEnd());
        }

        public void StartSlideCD()
        { 
            StartCoroutine(SlideCooldown());
        }

        IEnumerator SlideCooldown()
        {
            CanSlide = false;
            yield return new WaitForSeconds(2f);
            CanSlide = true;
        }


        /// <summary>
        /// Kills the momentum gained by slide jumping so you dont go into orbit.
        /// </summary>
        /// <returns></returns>
        IEnumerator SlideJumpEnd()// I put this here because I want this logic to continu even after no longer being in the sliding state.
        {
            yield return new WaitForSeconds(0.4f);
            CurrentMoveSpeed = SprintMoveSpeed;
        }

        public override void TakeDamageServerRpc(float damage)
        {
            base.TakeDamageServerRpc(damage);
            EntityAbility.HasTakenDmg = true;
            Debug.Log(gameObject);
        }
    }
}
