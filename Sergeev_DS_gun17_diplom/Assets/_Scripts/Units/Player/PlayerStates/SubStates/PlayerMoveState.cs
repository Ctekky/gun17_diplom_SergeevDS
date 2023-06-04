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
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (IsExitingState) return;
            if (InputX == 0 && InputY != -1)
            {
                StateMachine.ChangeState(Player.IdleState);
            }
            else if(InputX == 0 && InputY == -1)
            {
                StateMachine.ChangeState(Player.CrouchIdleState);
            }
            else if(InputY == -1)
            {
                StateMachine.ChangeState(Player.CrouchMoveState);
            }
        }
        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            Movement?.CheckFlip(InputX);
            Movement?.SetVelocityX(PlayerData.movementVelocity * InputX);
        }
    }
}

