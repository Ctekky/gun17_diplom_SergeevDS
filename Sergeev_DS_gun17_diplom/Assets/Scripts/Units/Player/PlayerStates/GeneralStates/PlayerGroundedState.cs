using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Metroidvania.Player
{
    public class PlayerGroundedState : PlayerState
    {
        protected int inputX;
        protected int inputY;
        private bool jumpInput;
        private bool isGrounded;
        public PlayerGroundedState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
        }

        public override void DoChecks()
        {
            base.DoChecks();
            isGrounded = player.CheckIfGrounded();
        }

        public override void Enter()
        {
            base.Enter();
            player.JumpState.ResetJumps();
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
            jumpInput = player.InputHandler.JumpInput;

            if(jumpInput && player.JumpState.CanJump())
            {
                player.InputHandler.UseJumpInput();
                stateMachine.ChangeState(player.JumpState);
            }
            else if(!isGrounded)
            {
                player.InAirState.StartLastMomentJumpTimer();
                stateMachine.ChangeState(player.InAirState);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }
    }
}

