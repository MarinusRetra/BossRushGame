using UnityEngine;
using BossRush.Managers;
using System;
using Unity.Netcode;
using BossRush.FiniteStateMachine;
using BossRush.FiniteStateMachine.Entities;

namespace BossRush.Controllers
{
    public class PlayerController : NetworkBehaviour
    {
        [Header("General")]
        [SerializeField] PlayerEntity playerEntity;

        [SerializeField] private Transform parentTransform;
        InputManager input;

        [SerializeField] private Rigidbody rb;
        [SerializeField] private Canvas canvas;
        [SerializeField] private float sensivityY = 0.5f; // [TEMP] Will later be moved to a camera controller.

        [Header("Player Movement")]
        public int MoveSpeed = 20;
        public int sprintMultiplier = 2;
        public float jumpHeight = 10;

        private int normalMoveSpeed;
        private int sprintMoveSpeed;
        private float /* xRotation, */ yRotation;
        private Vector3 moveDirection;

        float xInput, yInput;

        void Start()
        {



            if (!IsOwner)
                return;
            input = InputManager.Instance;
            
            normalMoveSpeed = MoveSpeed;
            sprintMoveSpeed = MoveSpeed * sprintMultiplier;

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
             moveDirection = parentTransform.right * xInput + parentTransform.forward * yInput;
            
             rb.linearVelocity = MoveSpeed * moveDirection + new Vector3(0, rb.linearVelocity.y, 0);
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
             MoveSpeed = sprintMoveSpeed;
        }

        private void SprintCancelled()
        {
            MoveSpeed = normalMoveSpeed;
        }

        private void Look(Vector2 obj)
        {
            //xRotation += obj.y * sensivityX;
            yRotation += obj.x * sensivityY;

            //xRotation -= obj.y;
            
            playerEntity.transform.rotation = Quaternion.Euler(0, yRotation, 0);
        }

        private void Jump()
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpHeight,rb.linearVelocity.z);
        }

        private void JumpCancelled()
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y * .5f, rb.linearVelocity.z);
        }

        private void CrouchCancelled()
        {
            
        }

        private void Crouch()
        {
            MoveSpeed = MoveSpeed <= normalMoveSpeed ? MoveSpeed = normalMoveSpeed / 2 : MoveSpeed = sprintMoveSpeed * 2;
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
            //if (!IsOwner)
            //    return;
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
           // if (!IsOwner)
           //     return;
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
            // if (!IsOwner)
            //     return;
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
