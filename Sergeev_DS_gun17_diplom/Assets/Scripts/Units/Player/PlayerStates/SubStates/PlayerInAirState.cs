using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Metroidvania.Player
{
    public class PlayerInAirState : PlayerState
    {
        private int inputX;
        private bool isGrounded;
        private bool jumpInput;
        private bool jumpInputStop;
        private bool lastMomentJump;
        private bool isJumping;
        public PlayerInAirState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
        }

        public override void DoChecks()
        {
            base.DoChecks();
            isGrounded = player.CheckIfGrounded();
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            CheckLastMomentJump();
            inputX = player.InputHandler.NormalizedInputX;
            jumpInput = player.InputHandler.JumpInput;
            jumpInputStop = player.InputHandler.JumpInputStop;

            CheckJumpMultiplier();

            if (isGrounded && player.CurrentVelocity.y < 0.01f)
            {
                stateMachine.ChangeState(player.LandState);
            }
            else if(jumpInput && player.JumpState.CanJump())
            {
                 stateMachine.ChangeState(player.JumpState);
            }
            else
            {
                player.CheckFlip(inputX);
                player.SetVelocityX(playerData.movementVelocity * inputX);
                player.Animator.SetFloat("velocityY", player.CurrentVelocity.y);
            }
        }

        private void CheckJumpMultiplier()
        {
            if (isJumping)
            {
                if (jumpInputStop)
                {
                    player.SetVelocityY(player.CurrentVelocity.y * playerData.jumpHeighMultiplier);
                    isJumping = false;
                }
                else if (player.CurrentVelocity.y <= 0f)
                {
                    isJumping = false;
                }
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }
        private void CheckLastMomentJump()
        {
            if (lastMomentJump && Time.time > startTime + playerData.lastMomentJumpTime)
            {
                lastMomentJump = false;
                player.JumpState.DecreaseJumps();
            }
        }
        public void StartLastMomentJumpTimer() => lastMomentJump = true;
        public void SetIsJumping() => isJumping = true;
    }
}

