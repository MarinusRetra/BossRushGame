using BossRush.FiniteStateMachine.Entities;
using BossRush.Managers;
using UnityEngine;

namespace BossRush.FiniteStateMachine.Behaviors.MovementStates
{
    public class IdleState : State
    {
        InputManager input;
        PlayerEntity playerEntity;
        public IdleState(StateMachine machine) : base(machine)
        {
            playerEntity = (PlayerEntity)machine.GetEntity();
            input = InputManager.Instance;
        }
        public override void Enter()
        {
            input.MoveEvent += Move;
            input.PrimaryEvent += Primary;
            input.SecondaryEvent += Secondary;
            input.TertiaryEvent += Tertiary;
            input.BasicAttackEvent += BasicAttack;  
            input.CrouchEvent += Crouch;
            input.JumpEvent += Jump;
            input.SprintEvent += Sprint;
            input.LookEvent += Look;
            input.PauseEvent += Pause;

            playerEntity.CurrentMoveSpeed = playerEntity.WalkingMoveSpeed;

            Debug.Log("Entered IdleState");
        }

        public override void FixedUpdate()
        { 
            playerEntity.isGrounded = Physics.Raycast(playerEntity.transform.position, Vector3.down, playerEntity.groundCheckDistance, playerEntity.groundLayer);
            if (!playerEntity.isGrounded)
                playerEntity.Machine.SetState(playerEntity.FallingState);
        }

        public override void Exit()
        {
            input.MoveEvent -= Move;
            input.PrimaryEvent -= Primary;
            input.SecondaryEvent -= Secondary;
            input.TertiaryEvent -= Tertiary;
            input.BasicAttackEvent -= BasicAttack;
            input.CrouchEvent -= Crouch;
            input.JumpEvent -= Jump;
            input.SprintEvent -= Sprint;
            input.LookEvent -= Look;
            input.PauseEvent -= Pause;
        }
        private void Pause()
        {
           Machine.SetState(playerEntity.UIState);
        }

        private void Look(Vector2 vector)
        {
            playerEntity.BaseLook(vector);
        }

        private void Sprint()
        {
            playerEntity.BaseSprint();
        }

        private void Jump()
        {
            playerEntity.BaseJump();
        }

        private void Crouch()
        {
            playerEntity.BaseCrouch();
        }

        private void BasicAttack()
        {
            playerEntity.BaseBasicAttack();
        }

        private void Tertiary()
        {
            playerEntity.BaseTertiary();
        }

        private void Secondary()
        {
            playerEntity.BaseSecondary();
        }

        private void Primary()
        {
            playerEntity.BasePrimary();
        }

        private void Move(Vector2 vector)
        {
            playerEntity.Machine.SetState(playerEntity.WalkingState);
        }

    }
}
