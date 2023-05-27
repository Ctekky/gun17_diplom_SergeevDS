using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Metroidvania.Player
{
    public class PlayerLandState : PlayerGroundedState
    {
        public PlayerLandState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (isExitingState) return;
            if (inputX != 0)
            {
                stateMachine.ChangeState(player.MoveState);
            }
            else if (isAnimationEnd)
            {
                stateMachine.ChangeState(player.IdleState);
            }
        }
    }
}

