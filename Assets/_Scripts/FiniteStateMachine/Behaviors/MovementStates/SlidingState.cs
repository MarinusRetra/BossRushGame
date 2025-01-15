using BossRush.FiniteStateMachine.Entities;
using BossRush.Managers;
using System;
using Unity.Netcode;
using UnityEngine;

namespace BossRush.FiniteStateMachine.Behaviors.MovementStates
{
    public class SlidingState : State// Sliding state can enter UI, Running, Jumping and Walking state
    {
        PlayerEntity playerEntity;
        InputManager input;

        public SlidingState(StateMachine machine) : base(machine)
        {
            playerEntity = (PlayerEntity)machine.GetEntity();
            input = InputManager.Instance;
        }
        public override void Enter()
        {
            input.SprintEventCancelled += SprintCancelled;
            input.PrimaryEvent += Primary;
            input.SecondaryEvent += Secondary;
            input.TertiaryEvent += Tertiary;
            input.BasicAttackEvent += BasicAttack;
            input.CrouchEventCancelled += CrouchCancelled;
            input.JumpEvent += Jump;
            input.LookEvent += Look;
            input.PauseEvent += Pause;

            playerEntity.CurrentMoveSpeed = playerEntity.SlideMoveSpeed;

            playerEntity.playerRenderer.material.color = Color.yellow; // [Temp]

            Debug.Log("Entered SlidingState");
        }

        [ServerRpc(RequireOwnership = false)]
        public override void FixedUpdate()
        {
            // playerEntity.moveDirection = playerEntity.transform.right * playerEntity.xInput + playerEntity.transform.forward * playerEntity.yInput;
            // moveDirection wordt niet gezet want ik wil niet dat je beweegt tijdens een slide

            playerEntity.Body.linearVelocity = playerEntity.CurrentMoveSpeed * playerEntity.moveDirection + new Vector3(0, playerEntity.Body.linearVelocity.y, 0);

            if (!playerEntity.isGrounded)
                playerEntity.Machine.SetState(playerEntity.FallingState);
        }

        public override void Exit()
        {
            input.PrimaryEvent -= Primary;
            input.SecondaryEvent -= Secondary;
            input.TertiaryEvent -= Tertiary;
            input.BasicAttackEvent -= BasicAttack;
            input.CrouchEventCancelled -= CrouchCancelled;
            input.JumpEvent -= Jump;
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

        private void Jump()
        {
            playerEntity.CurrentMoveSpeed = playerEntity.SlideJumpMoveSpeed;

            playerEntity.Body.linearVelocity = playerEntity.CurrentMoveSpeed * playerEntity.moveDirection + new Vector3(0, playerEntity.Body.linearVelocity.y, 0); // Adds horizontal speed
            playerEntity.Body.linearVelocity = new Vector3(playerEntity.Body.linearVelocity.x, playerEntity.JumpHeight, playerEntity.Body.linearVelocity.z); // Adds vertical speed

            //playerEntity.Machine.SetState(playerEntity.JumpingState);
        }

        private void CrouchCancelled()
        {
            playerEntity.Machine.SetState(playerEntity.RunningState);
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
        private void SprintCancelled()
        {
            playerEntity.Machine.SetState(playerEntity.WalkingState);
        }
    }
}
