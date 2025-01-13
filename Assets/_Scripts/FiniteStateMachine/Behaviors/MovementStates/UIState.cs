using BossRush.FiniteStateMachine.Behaviors;
using BossRush.FiniteStateMachine.Entities;
using BossRush.Managers;
using UnityEngine;

namespace BossRush.FiniteStateMachine
{
    public class UIState : State
    {
        PlayerEntity playerEntity;
        Canvas canvas;
        InputManager input;

        public UIState(StateMachine machine) : base(machine) 
        {
            input = InputManager.Instance;
            playerEntity = (PlayerEntity)Machine.GetEntity();
            canvas = playerEntity.UICanvas;
        }

        public override void Enter()
        {
            //input.PointEvent += UI_Point;
            //input.NavigateEvent += UI_Navigate;
            input.ResumeEvent += UI_Resume;
            //input.ClickEvent += UI_Click;

            canvas.enabled = true;

            Debug.Log("Entered UIState");
        }

        public override void FixedUpdate()
        {
            playerEntity.isGrounded = Physics.Raycast(playerEntity.transform.position, Vector3.down, playerEntity.groundCheckDistance, playerEntity.groundLayer);
        }

        public override void Exit()
        {
            //input.PointEvent -= UI_Point;
            //input.NavigateEvent -= UI_Navigate;
            input.ResumeEvent -= UI_Resume;
            //input.ClickEvent -= UI_Click;

            canvas.enabled = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        //private void UI_Click()
        //{
        //}

        private void UI_Resume()
        {

            Machine.ResetToPreviousState(); // Thanks awesome function
        }

        //private void UI_Navigate(Vector2 vector)
        // {
        // }

        //private void UI_Point(Vector2 vector)
        // {
        // }
    }
}
