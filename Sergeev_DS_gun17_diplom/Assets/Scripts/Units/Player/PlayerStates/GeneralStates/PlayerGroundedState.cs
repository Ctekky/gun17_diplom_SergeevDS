using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Metroidvania.Player
{
    public class PlayerGroundedState : PlayerState
    {
        protected int inputX;
        protected int inputY;
        public PlayerGroundedState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
        }

        public override void DoChecks()
        {
            base.DoChecks();
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
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }
    }
}

