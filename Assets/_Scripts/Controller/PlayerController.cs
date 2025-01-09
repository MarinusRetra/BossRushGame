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
            input.MoveEvent += Input_MoveEvent;
            input.Ability1Event += Input_Ability1Event;
            input.Ability2Event += Input_Ability2Event;
            input.Ability3Event += Input_Ability3Event;
            input.BasicAttackEvent += Input_BasicAttackEvent;
            input.CrouchEvent += Input_CrouchEvent;
            input.CrouchEventCancelled += Input_CrouchEventCancelled;
            input.JumpEvent += Input_JumpEvent;
            input.JumpCancelledEvent += Input_JumpEventCancelled;
            input.SprintEvent += Input_SprintEvent;
            input.LookEvent += Input_LookEvent;
            input.PauseEvent += Input_PauseEvent;

            //UIActions
            input.PointEvent += UIInput_PointEvent;
            input.NavigateEvent += UIInput_NavigateEvent;
            input.ResumeEvent += UIInput_ResumeEvent;
            input.ClickEvent += UIInput_ClickEvent;
        }

        void Update()
        {
            
        }

        //These are called when the corresponding playeractions are called
        private void Input_PauseEvent()
        {
            Debug.Log("Input_PauseEvent");
        }

        private void Input_SprintEvent()
        {
            Debug.Log("Input_SprintEvent");
        }

        private void Input_LookEvent(Vector2 obj)
        {
            Debug.Log("Input_LookEvent");
        }
        private void Input_JumpEvent()
        {
            Debug.Log("Input_JumpEvent");
        }

        private void Input_JumpEventCancelled()
        {
            Debug.Log("Input_JumpEventCancelled");
        }

        private void Input_CrouchEventCancelled()
        {
            Debug.Log("Input_CrouchEventCancelled");
        }

        private void Input_CrouchEvent()
        {
            Debug.Log("Input_CrouchEvent");
        }


        private void Input_BasicAttackEvent()
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

        private void Input_MoveEvent(Vector2 obj)
        {
            Debug.Log("Input_MoveEvent");
        }
        
        //These are called when the corresponding UI events are called.
        private void UIInput_ClickEvent()
        {
            Debug.Log("UIInput_ClickEvent");
        }
        private void UIInput_ResumeEvent()
        {
            Debug.Log("UIInput_Clickevent");
        }
        private void UIInput_PointEvent(Vector2 obj)
        {
            Debug.Log("UIInput_PointEvent");
        }

        private void UIInput_NavigateEvent(Vector2 obj)
        {
            Debug.Log("UIInput_NavigateEvent");
        }
    }
}
