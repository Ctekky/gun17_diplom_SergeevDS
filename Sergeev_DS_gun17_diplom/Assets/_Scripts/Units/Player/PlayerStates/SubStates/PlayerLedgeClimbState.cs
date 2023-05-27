using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Metroidvania.BaseUnit;

namespace Metroidvania.Player
{
    public class PlayerLedgeClimbState : PlayerState
    {
        protected Movement Movement
        {
            get => movement ?? unit.GetUnitComponent<Movement>(ref movement);
        }
        private CollisionChecks CollisionChecks
        {
            get => collisionChecks ?? unit.GetUnitComponent<CollisionChecks>(ref collisionChecks);
        }
        private Movement movement;
        private CollisionChecks collisionChecks;
        private Vector2 detectedPos;
        private Vector2 cornerPos;
        private Vector2 startPos;
        private Vector2 stopPos;
        private Vector2 workVector;

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
            Movement?.SetVelocityZero();
            player.transform.position = detectedPos;
            cornerPos = DetermineCornerPositon();
            startPos.Set(cornerPos.x - (Movement.FacingDirection * playerData.startOffset.x), cornerPos.y - playerData.startOffset.y);
            stopPos.Set(cornerPos.x + (Movement.FacingDirection * playerData.stopOffset.x), cornerPos.y + playerData.stopOffset.y);

            player.transform.position = startPos;
        }

        public override void Exit()
        {
            base.Exit();
            isHanging = false;
            if (isClimbing)
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
                if (isTouchHead)
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

                Movement?.SetVelocityZero();
                player.transform.position = startPos;

                if (inputX == Movement?.FacingDirection && isHanging && !isClimbing)
                {
                    CheckForSpace();
                    isClimbing = true;
                    player.Animator.SetBool("climbLedge", true);
                }
                else if (inputY == -1 && isHanging && !isClimbing)
                {
                    stateMachine.ChangeState(player.InAirState);
                }
                else if (jumpInput && !isClimbing)
                {
                    player.WallJumpState.WallJumpDirection(true);
                    stateMachine.ChangeState(player.WallJumpState);
                }
            }
        }

        public void SetDetectedPosition(Vector2 pos) => detectedPos = pos;
        private void CheckForSpace()
        {
            isTouchHead = Physics2D.Raycast(cornerPos + (Vector2.up * 0.015f) + (Vector2.right * Movement.FacingDirection * 0.015f),
                                            Vector2.up,
                                            playerData.colliderStandHeight,
                                            CollisionChecks.GroundLayer);
        }
        private Vector2 DetermineCornerPositon()
        {
            RaycastHit2D hitX = Physics2D.Raycast(CollisionChecks.WallCheck.position,
                                                  Vector2.right * Movement.FacingDirection,
                                                  CollisionChecks.WallCheckDistance,
                                                  CollisionChecks.GroundLayer);
            float distanceX = hitX.distance;
            workVector.Set(distanceX * Movement.FacingDirection, 0f);
            RaycastHit2D hitY = Physics2D.Raycast(CollisionChecks.LedgeCheckHorizontal.position + (Vector3)(workVector),
                                                  Vector2.down,
                                                  CollisionChecks.LedgeCheckHorizontal.position.y - CollisionChecks.WallCheck.position.y,
                                                  CollisionChecks.GroundLayer);
            float distanceY = hitY.distance;
            workVector.Set(CollisionChecks.WallCheck.position.x + (distanceX * Movement.FacingDirection), CollisionChecks.LedgeCheckHorizontal.position.y - distanceY);
            return workVector;
        }
    }
}

