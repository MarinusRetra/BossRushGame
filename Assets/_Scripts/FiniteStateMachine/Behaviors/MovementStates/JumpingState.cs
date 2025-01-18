using BossRush.FiniteStateMachine.Entities;
using BossRush.Managers;
using Unity.Netcode;
using UnityEngine;

namespace BossRush.FiniteStateMachine.Behaviors.MovementStates
{
    public class JumpingState : State // JumpingState can go to FallingState, UIState, WalkingState.
    {
        PlayerEntity playerEntity;
        InputManager input;

        public JumpingState(StateMachine machine) : base(machine)
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
            input.JumpCancelledEvent += JumpCancelled;
            input.LookEvent += Look;
            input.PauseEvent += Pause;

            if (!playerEntity.IsGrounded) //If you somehow manage to get here without grounded being true reset to previous state
            { 
                Machine.ResetToPreviousState();
            }

            playerEntity.CurrentMoveSpeed *= .9f;


            if (playerEntity.CurrentMoveSpeed == 0)
            {
                playerEntity.CurrentMoveSpeed = playerEntity.WalkingMoveSpeed;
            }

            Debug.Log("Entered JumpingState");
            
            playerEntity.IsGrounded = false;

            playerEntity.playerRenderer.material.color = Color.red; // [Temp]

            playerEntity.Body.linearVelocity = new Vector3(playerEntity.Body.linearVelocity.x, playerEntity.JumpHeight, playerEntity.Body.linearVelocity.z);// Jump logic.
        }

        [ServerRpc(RequireOwnership = false)]
        public override void FixedUpdate()
        {
            playerEntity.moveDirection = playerEntity.transform.right * playerEntity.xInput + playerEntity.transform.forward * playerEntity.yInput;// Sets move direction to WASD input.
            playerEntity.Body.linearVelocity = playerEntity.CurrentMoveSpeed * playerEntity.moveDirection + new Vector3(0, playerEntity.Body.linearVelocity.y, 0);// Applies CurrentMoveSpeed in moveDirection.
            
            if (playerEntity.Body.linearVelocity.y < 3)// Makes the player fall faster to make movement feeling less floaty.
            {
                playerEntity.Body.linearVelocity += (playerEntity.FallMultiplier) * Physics.gravity.y * Time.fixedDeltaTime * Vector3.up;
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
            input.JumpCancelledEvent -= JumpCancelled;
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

        private void JumpCancelled()
        {
            if (!playerEntity.TrueIsGrounded)
                Machine.SetState(playerEntity.FallingState);
            else
                Machine.SetState(playerEntity.WalkingState);
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

        private void Move(Vector2 vector)// Lowers the value from input so movement is a little more restricted while in air without killing your momentum.
        {
            playerEntity.xInput = vector.x * .9f;
            playerEntity.yInput = vector.y * .9f;
        }

    }
}
