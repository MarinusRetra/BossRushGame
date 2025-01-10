using UnityEngine;
using BossRush.Managers;

namespace BossRush.Controllers
{
    public class PlayerController : MonoBehaviour
    {
        void Start()
        {
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

        void Update()
        {
            
        }

        //These are called when the corresponding playeractions are called
        private void Pause()
        {
            Debug.Log("Input_PauseEvent");
        }

        private void Sprint()
        {
            Debug.Log("Input_SprintEvent");
        }

        private void Look(Vector2 obj)
        {
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
            Debug.Log("UIInput_Clickevent");
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
