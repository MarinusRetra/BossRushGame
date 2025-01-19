using BossRush.FiniteStateMachine.Entities;
using BossRush.Managers;
using Unity.Netcode;
using UnityEngine;

namespace BossRush.FiniteStateMachine.Behaviors.MovementStates
{
    public class CrouchingState : State
    {
        PlayerEntity playerEntity;
        InputManager input;  

        public CrouchingState(StateMachine machine) : base(machine)
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
            input.CrouchEventCancelled += CrouchCancelled;
            input.JumpEvent += Jump;
            input.SprintEvent += Sprint;
            input.LookEvent += Look;
            input.PauseEvent += Pause;

            playerEntity.playerRenderer.material.color = Color.clear;

            Debug.Log("Entered CrouchingState");
        }
      
        [ServerRpc(RequireOwnership = false)]
        public override void FixedUpdate()
        {
            playerEntity.moveDirection = playerEntity.transform.right * playerEntity.xInput + playerEntity.transform.forward * playerEntity.yInput;

            playerEntity.Body.linearVelocity = playerEntity.CrouchingMoveSpeed * playerEntity.moveDirection + new Vector3(0, playerEntity.Body.linearVelocity.y, 0);
        }

        public override void Exit()
        {
            input.MoveEvent -= Move;
            input.PrimaryEvent -= Primary;
            input.SecondaryEvent -= Secondary;
            input.TertiaryEvent -= Tertiary;
            input.BasicAttackEvent -= BasicAttack;
            input.CrouchEventCancelled -= CrouchCancelled;
            input.JumpEvent -= Jump;
            input.SprintEvent -= Sprint;
            input.LookEvent -= Look;
            input.PauseEvent -= Pause;
        }

        private void Pause()
        {
            playerEntity.BasePause();
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
            if(playerEntity.IsGrounded) // You can crouch in the air so this check is needed
            playerEntity.BaseJump();
        }

        private void CrouchCancelled()
        {
            Machine.SetState(playerEntity.WalkingState);
        }

        private void BasicAttack()
        {
            playerEntity.BasePrimary();
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
            playerEntity.BaseMove(vector);
        }

    }
}
