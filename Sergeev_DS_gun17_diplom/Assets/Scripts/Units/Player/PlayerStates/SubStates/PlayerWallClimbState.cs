using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Metroidvania.Player
{
    public class PlayerWallClimbState : PlayerWallTouchState
    {
        public PlayerWallClimbState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
        }
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (isExitingState) return;
            player.SetVelocityX(0f);
            player.SetVelocityY(playerData.wallClimbVelocity);
            if (inputY != 1)
            {
                stateMachine.ChangeState(player.WallGrabState);
            }
        }
    }

}