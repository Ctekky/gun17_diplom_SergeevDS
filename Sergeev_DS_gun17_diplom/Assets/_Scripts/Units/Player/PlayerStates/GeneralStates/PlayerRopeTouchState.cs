using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Metroidvania.BaseUnit;

namespace Metroidvania.Player
{
    public class PlayerRopeTouchState : PlayerState
    {
        protected bool IsGrounded;
        protected bool IsTouchingWall;
        protected bool IsTouchingRope;
        protected bool InteractInput;
        protected int InputX;
        protected int InputY;
        protected bool JumpInput;
        protected Movement Movement
        {
            get => _movement ?? Unit.GetUnitComponent<Movement>(ref _movement);
        }
        private CollisionChecks CollisionChecks
        {
            get => _collisionChecks ?? Unit.GetUnitComponent<CollisionChecks>(ref _collisionChecks);
        }
        private Movement _movement;
        private CollisionChecks _collisionChecks;

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
                IsGrounded = CollisionChecks.Grounded;
                IsTouchingWall = CollisionChecks.WallFront;
                IsTouchingRope = Player.IsTouchingRope;
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
            InputX = Player.InputHandler.NormalizedInputX;
            InputY = Player.InputHandler.NormalizedInputY;
            InteractInput = Player.InputHandler.InteractInput;
            JumpInput = Player.InputHandler.JumpInput;
            if (JumpInput)
            {
                Player.WallJumpState.WallJumpDirection(IsTouchingWall);
                StateMachine.ChangeState(Player.WallJumpState);
            }
            else if (IsGrounded && !InteractInput)
            {
                StateMachine.ChangeState(Player.IdleState);
            }
            else if (!IsTouchingRope || (InputX != Movement?.FacingDirection && !InteractInput))
            {
                StateMachine.ChangeState(Player.InAirState);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }
    }
}

