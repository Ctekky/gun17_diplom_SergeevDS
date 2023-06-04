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
            if (IsExitingState) return;
            if (InputX != 0)
            {
                StateMachine.ChangeState(Player.MoveState);
            }
            else if (IsAnimationEnd)
            {
                StateMachine.ChangeState(Player.IdleState);
            }
        }
    }
}

