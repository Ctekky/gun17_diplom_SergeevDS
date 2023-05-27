using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Metroidvania.Player
{
    public class PlayerWallJumpState : PlayerAbilityState
    {
        private int wallJumpDirection;
        public PlayerWallJumpState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();
            player.InputHandler.UseJumpInput();
            player.JumpState.ResetJumps();
            Movement?.SetVelocity(playerData.wallJumpVelocity, playerData.wallJumpAngle, wallJumpDirection);
            Movement?.CheckFlip(wallJumpDirection);
            player.JumpState.DecreaseJumps();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            player.Animator.SetFloat("velocityY", Movement.CurrentVelocity.y);

            if (Time.time >= startTime + playerData.wallJumpTime)
            {
                isAbilityDone = true;
            }
        }

        public void WallJumpDirection(bool isTouchingWall)
        {
            if (isTouchingWall)
            {
                wallJumpDirection = -Movement.FacingDirection;
            }
            else
            {
                wallJumpDirection = Movement.FacingDirection;
            }
        }
    }
}
