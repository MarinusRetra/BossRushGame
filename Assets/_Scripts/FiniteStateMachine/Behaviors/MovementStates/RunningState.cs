using BossRush.FiniteStateMachine.Entities;
using BossRush.Managers;
using System;
using UnityEngine;

namespace BossRush.FiniteStateMachine.Behaviors.MovementStates
{
    public class RunningState : State
    {
        [SerializeField] PlayerEntity playerEntity;
        InputManager input;
        public RunningState(StateMachine machine) : base(machine)
        {
            input = InputManager.Instance;

        }
        public override void Enter()
        {
            input.MoveEvent += Move;
            input.PrimaryEvent += Input_Ability1Event;
            input.SecondaryEvent += Input_Ability2Event;
            input.TertiaryEvent += Input_Ability3Event;
            input.BasicAttackEvent += BasicAttack;
            input.CrouchEvent += Crouch;
            input.JumpEvent += Jump;
            input.LookEvent += Look;
            input.PauseEvent += Pause;
            input.SprintEventCancelled += SprintCancelled;

            playerEntity.CurrentMoveSpeed = playerEntity.SprintMoveSpeed;
        }
        public override void FixedUpdate()
        {

        }
        public override void Exit()
        {
            input.MoveEvent -= Move;
            input.PrimaryEvent -= Input_Ability1Event;
            input.SecondaryEvent -= Input_Ability2Event;
            input.TertiaryEvent -= Input_Ability3Event;
            input.BasicAttackEvent -= BasicAttack;
            input.CrouchEvent -= Crouch;
            input.JumpEvent -= Jump;
            input.LookEvent -= Look;
            input.PauseEvent -= Pause;
            input.SprintEventCancelled -= SprintCancelled;
        }

        private void SprintCancelled()
        {
            throw new NotImplementedException();
        }

        private void Pause()
        {
            throw new NotImplementedException();
        }

        private void Look(Vector2 vector)
        {
            throw new NotImplementedException();
        }
        private void Jump()
        {
            throw new NotImplementedException();
        }

        private void Crouch()
        {
            throw new NotImplementedException();
        }

        private void BasicAttack()
        {
            throw new NotImplementedException();
        }

        private void Input_Ability3Event()
        {
            throw new NotImplementedException();
        }

        private void Input_Ability2Event()
        {
            throw new NotImplementedException();
        }

        private void Input_Ability1Event()
        {
            throw new NotImplementedException();
        }

        private void Move(Vector2 vector)
        {
            throw new NotImplementedException();
        }

    }
}
