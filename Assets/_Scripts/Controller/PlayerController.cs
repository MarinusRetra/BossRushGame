using UnityEngine;
using BossRush.Managers;
using Unity.Netcode;
using BossRush.FiniteStateMachine.Entities;

namespace BossRush.Controllers
{
    public class PlayerController : NetworkBehaviour
    {
        [Header("General")]
        [SerializeField] PlayerEntity playerEntity;

        InputManager input;

        [SerializeField] private Canvas canvas;

        [Header("Player Movement")]

        float xInput, yInput;

        void Start()
        {
            if (!IsOwner)
                return;
            input = InputManager.Instance;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

        }

        private void FixedUpdate()
        {
            if (!IsOwner)
                return;
            MovementServerRpc();
        }

        [ServerRpc(RequireOwnership = false)]
        public void MovementServerRpc()
        {
             playerEntity.moveDirection = playerEntity.transform.right * xInput + playerEntity.transform.forward * yInput;
            
             playerEntity.Body.linearVelocity = playerEntity.CurrentMoveSpeed * playerEntity.moveDirection + new Vector3(0, playerEntity.Body.linearVelocity.y, 0);
        }


        // These are called when the corresponding playeraction event is called.
        private void Move(Vector2 obj)
        {
            xInput = obj.x;
            yInput = obj.y;
            Debug.Log(obj.x + "" + obj.y);
        }
        private void Pause()
        {
            canvas.enabled = true;
        }

        private void Sprint()
        {
        }

        private void SprintCancelled()
        {
        }

        private void Look(Vector2 obj)
        {
            playerEntity.yRotation += obj.x * playerEntity.sensivityY;
            
            playerEntity.transform.rotation = Quaternion.Euler(0, playerEntity.yRotation, 0);
        }

        private void Jump()
        {
            playerEntity.Body.linearVelocity = new Vector3(playerEntity.Body.linearVelocity.x, playerEntity.JumpHeight,playerEntity.Body.linearVelocity.z);
        }

        private void JumpCancelled()
        {
            playerEntity.Body.linearVelocity = new Vector3(playerEntity.Body.linearVelocity.x, playerEntity.Body.linearVelocity.y * .5f, playerEntity.Body.linearVelocity.z);
        }

        private void CrouchCancelled()
        {
        }

        private void Crouch()
        {
            playerEntity.CurrentMoveSpeed = playerEntity.CurrentMoveSpeed <= playerEntity.WalkingMoveSpeed ? playerEntity.CurrentMoveSpeed = playerEntity.CrouchingMoveSpeed : playerEntity.CurrentMoveSpeed = playerEntity.SlideMoveSpeed;
        }

        private void BasicAttack()
        {
        }

        private void Input_Ability1Event()
        {
        }

        private void Input_Ability2Event()
        {
        }

        private void Input_Ability3Event()
        {
        }
        
        //These are called when the corresponding UI events are called.
        private void UI_Click()
        {
        }
        private void UI_Resume()
        {
           canvas.enabled = false;
        }
        private void UI_Point(Vector2 obj)
        {
        }
        private void UI_Navigate(Vector2 obj)
        {
        }
        private void OnEnable()
        {
            input = InputManager.Instance;

            input.MoveEvent += Move;
            input.PrimaryEvent += Input_Ability1Event;
            input.SecondaryEvent += Input_Ability2Event;
            input.TertiaryEvent += Input_Ability3Event;
            input.BasicAttackEvent += BasicAttack;
            input.CrouchEvent += Crouch;
            input.CrouchEventCancelled += CrouchCancelled;
            input.JumpEvent += Jump;
            input.JumpCancelledEvent += JumpCancelled;
            input.SprintEvent += Sprint;
            input.LookEvent += Look;
            input.PauseEvent += Pause;
            input.SprintEventCancelled += SprintCancelled;
            input.PointEvent += UI_Point;
            input.NavigateEvent += UI_Navigate;
            input.ResumeEvent += UI_Resume;
            input.ClickEvent += UI_Click;
        }

        private void OnDisable()
        {
            input.MoveEvent -= Move;
            input.PrimaryEvent -= Input_Ability1Event;
            input.SecondaryEvent -= Input_Ability2Event;
            input.TertiaryEvent -= Input_Ability3Event;
            input.BasicAttackEvent -= BasicAttack;
            input.CrouchEvent -= Crouch;
            input.CrouchEventCancelled -= CrouchCancelled;
            input.JumpEvent -= Jump;
            input.JumpCancelledEvent -= JumpCancelled;
            input.SprintEvent -= Sprint;
            input.LookEvent -= Look;
            input.PauseEvent -= Pause;
            input.SprintEventCancelled -= SprintCancelled;
            input.PointEvent -= UI_Point;
            input.NavigateEvent -= UI_Navigate;
            input.ResumeEvent -= UI_Resume;
            input.ClickEvent -= UI_Click;
        }
        public override void OnDestroy()
        {
            input.MoveEvent -= Move;
            input.PrimaryEvent -= Input_Ability1Event;
            input.SecondaryEvent -= Input_Ability2Event;
            input.TertiaryEvent -= Input_Ability3Event;
            input.BasicAttackEvent -= BasicAttack;
            input.CrouchEvent -= Crouch;
            input.CrouchEventCancelled -= CrouchCancelled;
            input.JumpEvent -= Jump;
            input.JumpCancelledEvent -= JumpCancelled;
            input.SprintEvent -= Sprint;
            input.LookEvent -= Look;
            input.PauseEvent -= Pause;
            input.SprintEventCancelled -= SprintCancelled;
            input.PointEvent -= UI_Point;
            input.NavigateEvent -= UI_Navigate;
            input.ResumeEvent -= UI_Resume;
            input.ClickEvent -= UI_Click;
          
            base.OnDestroy();
        }
    }
}
