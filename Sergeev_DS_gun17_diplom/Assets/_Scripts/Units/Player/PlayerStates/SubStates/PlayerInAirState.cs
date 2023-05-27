using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Metroidvania.BaseUnit;

namespace Metroidvania.Player
{
    public class PlayerInAirState : PlayerState
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
        private int inputX;
        private bool isGrounded;
        private bool isTouchingWall;
        private bool isTouchingWallBack;
        private bool oldIsTouchingWall;
        private bool oldIsTouchingWallBack;
        private bool jumpInput;
        private bool jumpInputStop;
        private bool interactInput;
        private bool lastMomentJump;
        private bool lastMomentWallJump;
        private bool isJumping;
        private bool isTouchingRope;
        private bool isTouchingLedge;

        private float startLastMomentWallJumpTime;

        public PlayerInAirState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
        }

        public override void DoChecks()
        {
            base.DoChecks();
            oldIsTouchingWall = isTouchingWall;
            oldIsTouchingWallBack = isTouchingWallBack;

            if (CollisionChecks)
            {
                isGrounded = CollisionChecks.Grounded;
                isTouchingWall = CollisionChecks.WallFront;
                isTouchingWallBack = CollisionChecks.WallBack;
                isTouchingRope = player.IsTouchingRope;
                isTouchingLedge = CollisionChecks.LedgeHorizontal;

            }
            if (isTouchingWall && !isTouchingLedge)
            {
                player.LedgeClimbState.SetDetectedPosition(player.transform.position);
            }

            if (!lastMomentWallJump && !isTouchingWall && !isTouchingWallBack && (oldIsTouchingWall || oldIsTouchingWallBack))
            {
                StartLastMomentWallJumpTimer();
            }
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Exit()
        {
            base.Exit();
            oldIsTouchingWall = false;
            oldIsTouchingWallBack = false;
            isTouchingWall = false;
            isTouchingWallBack = false;

        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            CheckLastMomentJump();
            CheckLastMomentWallJump();

            inputX = player.InputHandler.NormalizedInputX;
            jumpInput = player.InputHandler.JumpInput;
            jumpInputStop = player.InputHandler.JumpInputStop;
            interactInput = player.InputHandler.InteractInput;

            CheckJumpMultiplier();
            if (player.InputHandler.AttackInputs[(int)CombatInputs.primary])
            {
                stateMachine.ChangeState(player.PrimaryAttackState);
            }
            else if (player.InputHandler.AttackInputs[(int)CombatInputs.secondary])
            {
                stateMachine.ChangeState(player.SecondaryAttackState);
            }
            else if (isGrounded && Movement?.CurrentVelocity.y < 0.01f)
            {
                stateMachine.ChangeState(player.LandState);
            }
            else if (isTouchingWall && !isTouchingLedge && !isGrounded)
            {
                stateMachine.ChangeState(player.LedgeClimbState);
            }
            else if (jumpInput && (isTouchingWall || isTouchingWallBack || lastMomentWallJump))
            {
                StoptLastMomentWallJumpTimer();
                isTouchingWall = CollisionChecks.WallFront;
                player.WallJumpState.WallJumpDirection(isTouchingWall);
                stateMachine.ChangeState(player.WallJumpState);
            }
            else if (jumpInput && player.JumpState.CanJump())
            {
                stateMachine.ChangeState(player.JumpState);
            }
            else if (isTouchingRope && interactInput)
            {
                stateMachine.ChangeState(player.RopeGrabState);
            }
            else if (isTouchingWall && interactInput && isTouchingLedge)
            {
                stateMachine.ChangeState(player.WallGrabState);
            }
            else if (isTouchingWall && inputX == Movement?.FacingDirection && Movement?.CurrentVelocity.y <= 0f)
            {
                stateMachine.ChangeState(player.WallSlideState);
            }
            else
            {
                Movement?.CheckFlip(inputX);
                Movement?.SetVelocityX(playerData.movementVelocity * inputX);
                player.Animator.SetFloat("velocityY", Movement.CurrentVelocity.y);
            }
        }

        private void CheckJumpMultiplier()
        {
            if (isJumping)
            {
                if (jumpInputStop)
                {
                    Movement?.SetVelocityY(Movement.CurrentVelocity.y * playerData.jumpHeighMultiplier);
                    isJumping = false;
                }
                else if (Movement?.CurrentVelocity.y <= 0f)
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
        private void CheckLastMomentWallJump()
        {
            if (lastMomentWallJump && Time.time > startLastMomentWallJumpTime + playerData.lastMomentJumpTime)
            {
                lastMomentWallJump = false;
            }
        }
        public void StartLastMomentJumpTimer() => lastMomentJump = true;
        public void SetIsJumping() => isJumping = true;
        public void StartLastMomentWallJumpTimer()
        {
            lastMomentWallJump = true;
            startLastMomentWallJumpTime = Time.time;
        }
        public void StoptLastMomentWallJumpTimer() => lastMomentWallJump = false;

    }
}

