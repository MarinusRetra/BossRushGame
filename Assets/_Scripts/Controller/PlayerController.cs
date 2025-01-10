using UnityEngine;
using BossRush.Managers;
using System;

namespace BossRush.Controllers
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] Canvas canvas;

        [SerializeField] readonly float sensivityX = 0.5f, sensivityY = 0.8f;
        private float xRotation, yRotation;

        private Transform playerTransform;
        [SerializeField] private Transform cameraTransform;

        void Start()
        {
            playerTransform = transform;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            //Reference naar de instance van de InputManager singleton
            InputManager input = InputManager.Instance;

            //PlayerActions
            input.MoveEvent += Move;
            input.PrimaryEvent += Input_Ability1Event;
            input.SecondaryEvent += Input_Ability2Event;
            input.Ability3Event += Input_Ability3Event;
            input.BasicAttackEvent += BasicAttack;
            input.CrouchEvent += Crouch;
            input.CrouchEventCancelled += CrouchCancelled;
            input.JumpEvent += Jump;
            input.JumpCancelledEvent += JumpCancelled;
            input.SprintEvent += Sprint;
            input.LookEvent += Look;
            input.PauseEvent += Pause;

            //UIActions
            input.PointEvent += UI_Point;
            input.NavigateEvent += UI_Navigate;
            input.ResumeEvent += UI_Resume;
            input.ClickEvent += UI_Click;
        }

        // These are called when the corresponding playeractions are called
        private void Pause()
        {
            canvas.enabled = true;
            Debug.Log("Input_PauseEvent");
        }

        private void Sprint()
        {
            Debug.Log("Sprint");

        }

        private void Look(Vector2 obj)
        {
            xRotation += obj.y * sensivityX;
            yRotation += obj.x * sensivityY;

            // xRotation is a float but we don't need float precision for clamping these values so i use the int version.
            xRotation = Math.Clamp(xRotation, -19, 19); // This clamp is temporary until we have a seperate camera controller.

            playerTransform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
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

        private void Move(Vector2 obj)
        {
            Debug.Log("Input_MoveEvent");
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
    }
}
