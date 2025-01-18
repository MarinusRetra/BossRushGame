using BossRush.FiniteStateMachine.Entities;
using BossRush.Managers;
using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace BossRush.FiniteStateMachine.Behaviors.MovementStates
{
    public class SlidingState : State// Sliding state can enter UI, Running, Jumping and Walking state
    {
        PlayerEntity playerEntity;
        InputManager input;
        float slideEndTimer = 0.6f;
        float slideEndCounter = 0;

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

            playerEntity.StartSlideCD();

            slideEndCounter = slideEndTimer;

            playerEntity.CurrentMoveSpeed = playerEntity.SlideMoveSpeed;

            playerEntity.playerRenderer.material.color = Color.yellow; // [Temp]

            Debug.Log("Entered SlidingState");
        }

        [ServerRpc(RequireOwnership = false)]
        public override void FixedUpdate()
        {
            slideEndCounter -= Time.deltaTime;

            if (!playerEntity.IsGrounded)
                playerEntity.Machine.SetState(playerEntity.FallingState);
            if (slideEndCounter < 0)
            {
                Machine.SetState(playerEntity.WalkingState);
            }

            // moveDirection does not get set here since i dont want you to walk normally while sliding.

            playerEntity.Body.linearVelocity = playerEntity.CurrentMoveSpeed * playerEntity.moveDirection + new Vector3(0, playerEntity.Body.linearVelocity.y, 0); // Applies CurrentMoveSpeed in moveDirection 
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

            playerEntity.Body.linearVelocity = playerEntity.CurrentMoveSpeed * playerEntity.moveDirection + new Vector3(0, playerEntity.Body.linearVelocity.y / 2, 0); // Adds forward speed
            playerEntity.SlideJumpCoroutine(); // Ends the slide after .6 seconds.
            playerEntity.BaseJump();
        }

        private void CrouchCancelled()
        {
            Machine.SetState(playerEntity.RunningState);
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
            Machine.SetState(playerEntity.WalkingState);
        }
    }
}
