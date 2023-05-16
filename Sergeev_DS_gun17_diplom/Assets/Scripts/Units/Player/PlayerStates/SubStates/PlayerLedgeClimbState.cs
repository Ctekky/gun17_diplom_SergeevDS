using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Metroidvania.Player
{
    public class PlayerLedgeClimbState : PlayerState
    {
        private Vector2 detectedPos;
        private Vector2 cornerPos;
        private Vector2 startPos;
        private Vector2 stopPos;

        private bool isHanging;
        private bool isClimbing;
        private bool isTouchHead;

        private int inputX;
        private int inputY;
        private bool jumpInput;

        public PlayerLedgeClimbState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
        }

        public override void AnimationEndTrigger()
        {
            base.AnimationEndTrigger();
            player.Animator.SetBool("climbLedge", false);
        }

        public override void AnimationTrigger()
        {
            base.AnimationTrigger();
            isHanging = true;
            isClimbing = false;
        }

        public override void Enter()
        {
            base.Enter();
            player.SetVelocityZero();
            player.transform.position = detectedPos;
            cornerPos = player.DetermineCornerPositon();
            startPos.Set(cornerPos.x - (player.FacingDirection * playerData.startOffset.x), cornerPos.y - playerData.startOffset.y);
            stopPos.Set(cornerPos.x + (player.FacingDirection * playerData.stopOffset.x), cornerPos.y + playerData.stopOffset.y);

            player.transform.position = startPos;
        }

        public override void Exit()
        {
            base.Exit();
            isHanging = false; 
            if(isClimbing)
            {
                player.transform.position = stopPos;
                isClimbing = false;
            }
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (isAnimationEnd)
            {
                if(isTouchHead)
                {
                    stateMachine.ChangeState(player.CrouchIdleState);
                }
                else
                {
                    stateMachine.ChangeState(player.IdleState);
                }
            }
            else
            {
                inputX = player.InputHandler.NormalizedInputX;
                inputY = player.InputHandler.NormalizedInputY;
                jumpInput = player.InputHandler.JumpInput;

                player.SetVelocityZero();
                player.transform.position = startPos;

                if (inputX == player.FacingDirection && isHanging && !isClimbing)
                {
                    CheckForSpace();
                    isClimbing = true;
                    player.Animator.SetBool("climbLedge", true);
                }
                else if (inputY == -1 && isHanging && !isClimbing)
                {
                    stateMachine.ChangeState(player.InAirState);
                }
                else if(jumpInput && !isClimbing)
                {
                    player.WallJumpState.WallJumpDirection(true);
                    stateMachine.ChangeState(player.WallJumpState);
                }
            }
        }

        public void SetDetectedPosition(Vector2 pos) => detectedPos = pos;
        private void CheckForSpace()
        {
            isTouchHead = Physics2D.Raycast(cornerPos + (Vector2.up * 0.015f) + (Vector2.right * player.FacingDirection * 0.015f), Vector2.up, playerData.colliderStandHeight, playerData.groundLayer);
        }
    }
}

