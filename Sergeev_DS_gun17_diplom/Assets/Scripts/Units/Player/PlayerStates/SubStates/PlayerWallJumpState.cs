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
            player.SetVelocity(playerData.wallJumpVelocity, playerData.wallJumpAngle, wallJumpDirection);
            player.CheckFlip(wallJumpDirection);
            player.JumpState.DecreaseJumps();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            player.Animator.SetFloat("velocityY", player.CurrentVelocity.y);

            if (Time.time >= startTime + playerData.wallJumpTime)
            {
                isAbilityDone = true;
            }
        }

        public void WallJumpDirection(bool isTouchingWall)
        {
            if (isTouchingWall)
            {
                wallJumpDirection = -player.FacingDirection;
            }
            else
            {
                wallJumpDirection = player.FacingDirection;
            }
        }
    }
}
