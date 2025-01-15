using BossRush.FiniteStateMachine.Entities;
using BossRush.Managers;
using Unity.Netcode;
using UnityEngine;

namespace BossRush.FiniteStateMachine.Behaviors.MovementStates
{
    public class RunningState : State // RunningState can enter UI, Walking, Falling state, Sliding and jumping state
    {
        PlayerEntity playerEntity;
        InputManager input;
        public RunningState(StateMachine machine) : base(machine)
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
            input.LookEvent += Look;
            input.PauseEvent += Pause;
            input.SprintEventCancelled += SprintCancelled;

            playerEntity.CurrentMoveSpeed = playerEntity.SprintMoveSpeed;


            playerEntity.playerRenderer.material.color = Color.green; // [Temp]

            Debug.Log("Entered RunningState");
        }

        [ServerRpc(RequireOwnership = false)]
        public override void FixedUpdate()
        {
            playerEntity.moveDirection = playerEntity.transform.right * playerEntity.xInput + playerEntity.transform.forward * playerEntity.yInput;

            playerEntity.Body.linearVelocity = playerEntity.CurrentMoveSpeed * playerEntity.moveDirection + new Vector3(0, playerEntity.Body.linearVelocity.y, 0);

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
            input.LookEvent -= Look;
            input.PauseEvent -= Pause;
            input.SprintEventCancelled -= SprintCancelled;
        }

        private void SprintCancelled()
        {
            playerEntity.Machine.SetState(playerEntity.WalkingState);
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
            playerEntity.BaseJump();
        }

        private void Crouch()
        {
            if(playerEntity.isGrounded)
                playerEntity.Machine.SetState(playerEntity.SlidingState);
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
