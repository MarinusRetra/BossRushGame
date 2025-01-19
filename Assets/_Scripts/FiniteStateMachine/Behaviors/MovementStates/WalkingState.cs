using BossRush.FiniteStateMachine.Entities;
using BossRush.Managers;
using Unity.Netcode;
using UnityEngine;

namespace BossRush.FiniteStateMachine.Behaviors.MovementStates
{
    public class WalkingState : State // Walking State can enter Idle,Jumping,Crouching,Sprinting,UI, and Sprinting state
    {
        PlayerEntity playerEntity;
        InputManager input;
        public WalkingState(StateMachine machine) : base(machine)
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

            playerEntity.playerRenderer.material.color = Color.cyan; // [Temp]

            Debug.Log("Entered WalkingState");
        }

        [ServerRpc(RequireOwnership = false)]
        public override void FixedUpdate()
        {
            if (!playerEntity.IsGrounded)
            { 
                Machine.SetState(playerEntity.FallingState);
            }

            playerEntity.moveDirection = playerEntity.transform.right * playerEntity.xInput + playerEntity.transform.forward * playerEntity.yInput; // Sets move direction to WASD input

            playerEntity.Body.linearVelocity = playerEntity.CurrentMoveSpeed * playerEntity.moveDirection + new Vector3(0, playerEntity.Body.linearVelocity.y, 0); // Applies CurrentMoveSpeed in moveDirection 
            
            if (playerEntity.Body.linearVelocity == Vector3.zero) // Go to idle state when you stop moving
            {
                Machine.SetState(playerEntity.IdleState);
            }

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
            playerEntity.BaseMove(vector);
        }

    }
}
