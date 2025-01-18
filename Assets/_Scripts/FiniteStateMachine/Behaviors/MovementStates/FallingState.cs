using BossRush.FiniteStateMachine.Entities;
using BossRush.Managers;
using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Windows;

namespace BossRush.FiniteStateMachine.Behaviors.MovementStates
{
    public class FallingState : State // You can only enter WalkingState from FallingState
    {
        PlayerEntity playerEntity;
        InputManager input;
        bool holdingJump = false;
        public FallingState(StateMachine machine) : base(machine)
        {
            playerEntity = (PlayerEntity)machine.GetEntity();
            input = InputManager.Instance;
        }
        public override void Enter()
        {
            input.JumpCancelledEvent += JumpCancel;
            input.JumpEvent += Jump;
            input.MoveEvent += Move;
            input.PrimaryEvent += Primary;
            input.SecondaryEvent += Secondary;
            input.TertiaryEvent += Tertiary;
            input.BasicAttackEvent += BasicAttack;
            input.CrouchEvent += Crouch;
            input.LookEvent += Look;
            input.PauseEvent += Pause;

            playerEntity.CurrentMoveSpeed *= .9f;

            if (playerEntity.CurrentMoveSpeed == 0)
            { 
                playerEntity.CurrentMoveSpeed = playerEntity.WalkingMoveSpeed;
            }

            playerEntity.playerRenderer.material.color = Color.gray; // [Temp]

            if (playerEntity.Body.linearVelocity.y > 0)
            { 
                playerEntity.Body.linearVelocity = new Vector3(playerEntity.Body.linearVelocity.x, playerEntity.Body.linearVelocity.y * .4f, playerEntity.Body.linearVelocity.z);
            }

            Debug.Log("Entered FallingState");
        }


        [ServerRpc(RequireOwnership = false)]
        public override void FixedUpdate()
        {
            playerEntity.moveDirection = playerEntity.transform.right * playerEntity.xInput + playerEntity.transform.forward * playerEntity.yInput; // Sets move direction to WASD input.
            playerEntity.Body.linearVelocity = playerEntity.CurrentMoveSpeed * playerEntity.moveDirection + new Vector3(0, playerEntity.Body.linearVelocity.y, 0); // Applies CurrentMoveSpeed in moveDirection.


            if (playerEntity.Body.linearVelocity.y < 3) // Makes the player fall faster to make movement feeling less floaty.
            {
                playerEntity.Body.linearVelocity += (playerEntity.FallMultiplier) * Physics.gravity.y * Time.deltaTime * Vector3.up;
            }

            if (playerEntity.JumpBufferCounter > 0 && playerEntity.TrueIsGrounded) // You jump when JumpBufferCounter is higher than 0 and you're grounded.
            {
                if (!holdingJump) // Tap jump when not holding but pressing spacebar and does not enter the jumping state.
                {
                    playerEntity.IsGrounded = false;
                    playerEntity.Body.linearVelocity = new Vector3(playerEntity.Body.linearVelocity.x, playerEntity.JumpHeight, playerEntity.Body.linearVelocity.z); // Jump logic.
                    playerEntity.Body.linearVelocity = new Vector3(playerEntity.Body.linearVelocity.x, playerEntity.Body.linearVelocity.y * .4f, playerEntity.Body.linearVelocity.z); // Lowers up velocity to simulate entering fallingstate again
                }
                else // If you hold jump you will go to the jumping state and jump logic will happen in there.
                    playerEntity.BaseJump();
            }
            else if (playerEntity.JumpBufferCounter < 0 && playerEntity.TrueIsGrounded)
            { 
                Machine.SetState(playerEntity.WalkingState);
            }
        }


        public override void Exit()
        {
            input.JumpCancelledEvent -= JumpCancel;
            input.JumpEvent -= Jump;
            input.MoveEvent -= Move;
            input.PrimaryEvent -= Primary;
            input.SecondaryEvent -= Secondary;
            input.TertiaryEvent -= Tertiary;
            input.BasicAttackEvent -= BasicAttack;
            input.CrouchEvent -= Crouch;
            input.LookEvent -= Look;
            input.PauseEvent -= Pause;
        }
        private void JumpCancel()
        {
            holdingJump = false;
        }

        private void Jump()
        {
            playerEntity.JumpBufferCounter = playerEntity.JumpBufferTimer; // Sets the Jumpbuffer counter to the Jumpbuffertimer
            holdingJump = true;
        }

        private void Pause()
        {
            playerEntity.BasePause();
        }

        private void Look(Vector2 vector)
        {
            playerEntity.BaseLook(vector);
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

        private void Move(Vector2 vector) // Lowers the value from input so movement is a little more restricted while in air without killing your momentum.
        {
            playerEntity.xInput = vector.x * .9f;
            playerEntity.yInput = vector.y * .9f;
        }
    }
}
