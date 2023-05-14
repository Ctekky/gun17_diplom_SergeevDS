using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Metroidvania.Player
{
    public class PlayerRopeTouchState : PlayerState
    {
        protected bool isGrounded;
        protected bool isTouchingWall;
        protected bool isTouchingRope;
        protected bool interactInput;
        protected int inputX;
        protected int inputY;
        protected bool jumpInput;

        public PlayerRopeTouchState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
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
            isTouchingRope = player.IsTouchingRope;
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
            else if (!isTouchingRope || (inputX != player.FacingDirection && !interactInput))
            {
                stateMachine.ChangeState(player.InAirState);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }
    }
}

