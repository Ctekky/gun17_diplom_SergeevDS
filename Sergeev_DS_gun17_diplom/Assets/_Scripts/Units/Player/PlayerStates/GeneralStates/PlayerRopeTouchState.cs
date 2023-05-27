using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Metroidvania.BaseUnit;

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
            if (CollisionChecks != null)
            {
                isGrounded = CollisionChecks.Grounded;
                isTouchingWall = CollisionChecks.WallFront;
                isTouchingRope = player.IsTouchingRope;
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
            else if (!isTouchingRope || (inputX != Movement?.FacingDirection && !interactInput))
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

