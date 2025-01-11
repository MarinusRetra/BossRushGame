using UnityEngine;
using BossRush.Managers;
using System;

namespace BossRush.Controllers
{
    public class PlayerController : MonoBehaviour
    {
        [Header("General")]

        [SerializeField] private Transform parentTransform;
        private Transform playerTransform;
        InputManager input;

        [SerializeField] private Rigidbody rb;
        [SerializeField] private Canvas canvas;

        [Header("Player Movement")]
        public int MoveSpeed = 50;
        private int normalMoveSpeed;
        private int sprintMoveSpeed;

        private Vector3 moveDirection;
        float xInput, yInput;
        
        [Header("Zet deze later apart per player class")]
        [SerializeField] int sprintMultiplier = 2;

        [Header("Camera Movement")]

        [SerializeField] private Transform cameraTransform;
        [SerializeField] private readonly float sensivityX = 0.3f, sensivityY = 0.5f;
        private float xRotation, yRotation;


        void Start()
        {
            normalMoveSpeed = MoveSpeed;
            sprintMoveSpeed = MoveSpeed * sprintMultiplier;

            input = InputManager.Instance;
            rb.maxLinearVelocity = MoveSpeed;
            playerTransform = transform;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }



        private void Update()
        {
            rb.AddForce(MoveSpeed * moveDirection, ForceMode.Force);
            moveDirection = parentTransform.right * xInput + parentTransform.forward * yInput;
        }

        // These are called when the corresponding playeraction event is called.
        private void Move(Vector2 obj)
        {
            xInput = obj.x;
            yInput = obj.y;

            Debug.Log("Input_MoveEvent");
        }
        private void Pause()
        {
            canvas.enabled = true;
            Debug.Log("Input_PauseEvent");
        }

        private void Sprint()
        {
             MoveSpeed = sprintMoveSpeed;
        }

        private void SprintCancelled()
        {   
            MoveSpeed = normalMoveSpeed;
            Debug.Log("Sprint_EventCancelled");
        }

        private void Look(Vector2 obj)
        {
            xRotation += obj.y * sensivityX;
            yRotation += obj.x * sensivityY;

            xRotation -= obj.y;

            // xRotation is a float but we don't need float precision for clamping these values so i use the int version.
            xRotation = Math.Clamp(xRotation, -19, 19); // This clamp is temporary until we have a seperate camera controller.

            playerTransform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            parentTransform.rotation = Quaternion.Euler(0, yRotation, 0);
            Debug.Log("Input_LookEvent");
        }

        private void Jump()
        {
            Debug.Log("Input_JumpEvent");
        }

        private void JumpCancelled()
        {
            Debug.Log("Input_JumpEventCancelled");
        }

        private void CrouchCancelled()
        {
            Debug.Log("Input_CrouchEventCancelled");
        }

        private void Crouch()
        {
            Debug.Log("Input_CrouchEvent");
        }

        private void BasicAttack()
        {
            Debug.Log("Input_BasicAttackEvent");
        }

        private void Input_Ability1Event()
        {
            Debug.Log("Input_Ability1Event");
        }

        private void Input_Ability2Event()
        {
            Debug.Log("Input_Ability2Event");
        }

        private void Input_Ability3Event()
        {
            Debug.Log("Input_Ability3Event");
        }

        
        //These are called when the corresponding UI events are called.
        private void UI_Click()
        {
            Debug.Log("UIInput_ClickEvent");
        }
        private void UI_Resume()
        {
           canvas.enabled = false;
        }
        private void UI_Point(Vector2 obj)
        {
            Debug.Log("UIInput_PointEvent");
        }
        private void UI_Navigate(Vector2 obj)
        {
            Debug.Log("UIInput_NavigateEvent");
        }
        private void OnEnable()
        {
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
        private void OnDestroy()
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
    }
}
