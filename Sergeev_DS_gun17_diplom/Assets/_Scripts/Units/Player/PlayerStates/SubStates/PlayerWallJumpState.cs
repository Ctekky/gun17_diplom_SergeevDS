using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Metroidvania.Player
{
    public class PlayerWallJumpState : PlayerAbilityState
    {
        private int _wallJumpDirection;
        public PlayerWallJumpState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();
            Player.InputHandler.UseJumpInput();
            Player.JumpState.ResetJumps();
            Movement?.SetVelocity(PlayerData.wallJumpVelocity, PlayerData.wallJumpAngle, _wallJumpDirection);
            Movement?.CheckFlip(_wallJumpDirection);
            Player.JumpState.DecreaseJumps();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            Player.Animator.SetFloat("velocityY", Movement.CurrentVelocity.y);

            if (Time.time >= StartTime + PlayerData.wallJumpTime)
            {
                IsAbilityDone = true;
            }
        }

        public void WallJumpDirection(bool isTouchingWall)
        {
            if (isTouchingWall)
            {
                _wallJumpDirection = -Movement.FacingDirection;
            }
            else
            {
                _wallJumpDirection = Movement.FacingDirection;
            }
        }
    }
}
