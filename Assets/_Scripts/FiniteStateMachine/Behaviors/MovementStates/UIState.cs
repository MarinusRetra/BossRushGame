using BossRush.FiniteStateMachine.Behaviors;
using BossRush.Managers;
using System;
using UnityEngine;

namespace BossRush.FiniteStateMachine
{
    public class UIState : State
    {
        [SerializeField] Canvas canvas;
        InputManager input;

        public UIState(StateMachine machine) : base(machine) 
        {
            input = InputManager.Instance;
        }

        public override void Enter()
        {
            input.PointEvent += UI_Point;
            input.NavigateEvent += UI_Navigate;
            input.ResumeEvent += UI_Resume;
            input.ClickEvent += UI_Click;
           
            canvas.enabled = true;
        }

        public override void FixedUpdate()
        {

        }

        public override void Exit()
        {
            input.PointEvent += UI_Point;
            input.NavigateEvent += UI_Navigate;
            input.ResumeEvent += UI_Resume;
            input.ClickEvent += UI_Click;

            canvas.enabled = false;
        }

        private void UI_Click()
        {
            throw new NotImplementedException();
        }

        private void UI_Resume()
        {
            Machine.ResetToPreviousState(); // Thanks awesome function
        }

        private void UI_Navigate(Vector2 vector)
        {
            throw new NotImplementedException();
        }

        private void UI_Point(Vector2 vector)
        {
            throw new NotImplementedException();
        }
    }
}
