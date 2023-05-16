using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Metroidvania.Player
{
    public class PlayerMoveState : PlayerGroundedState
    {
        public PlayerMoveState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
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
            if (isExitingState) return;
            if (inputX == 0 && inputY != -1)
            {
                stateMachine.ChangeState(player.IdleState);
            }
            else if(inputX == 0 && inputY == -1)
            {
                stateMachine.ChangeState(player.CrouchIdleState);
            }
            else if(inputY == -1)
            {
                stateMachine.ChangeState(player.CrouchMoveState);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            player.CheckFlip(inputX);
            player.SetVelocityX(playerData.movementVelocity * inputX);
        }
    }
}

