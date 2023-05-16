using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Metroidvania.Player
{
    public class PlayerWallTouchState : PlayerState
    {
        protected bool isGrounded;
        protected bool isTouchingWall;
        protected bool interactInput;
        protected int inputX;
        protected int inputY;
        protected bool jumpInput;
        protected bool isTouchingLedge;


        public PlayerWallTouchState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
        }

        public override void AnimationEndTrigger()
        {
            base.AnimationEndTrigger();
        }

        public override void AnimationTrigger()
        {
            base.AnimationTrigger();
        }

        public override void DoChecks()
        {
            base.DoChecks();
            isGrounded = player.CheckIfGrounded();
            isTouchingWall = player.CheckIfTouchWall();
            isTouchingLedge = player.CheckIfTouchingLedge();

            if(isTouchingWall && !isTouchingLedge)
            {
                player.LedgeClimbState.SetDetectedPosition(player.transform.position);
            }

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
            inputX = player.InputHandler.NormalizedInputX;
            inputY = player.InputHandler.NormalizedInputY;
            interactInput = player.InputHandler.InteractInput;
            jumpInput = player.InputHandler.JumpInput;
            if (jumpInput)
            {
                player.WallJumpState.WallJumpDirection(isTouchingWall);
                stateMachine.ChangeState(player.WallJumpState);
            }
            else if (isGrounded && !interactInput)
            {
                stateMachine.ChangeState(player.IdleState);
            }
            else if (!isTouchingWall || (inputX != player.FacingDirection && !interactInput))
            {
                stateMachine.ChangeState(player.InAirState);
            }
            else if(isTouchingWall && !isTouchingLedge)
            {
                stateMachine.ChangeState(player.LedgeClimbState);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }
    }
}

