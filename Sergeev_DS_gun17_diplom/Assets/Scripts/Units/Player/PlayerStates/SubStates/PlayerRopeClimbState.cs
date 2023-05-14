using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Metroidvania.Player
{
    public class PlayerRopeClimbState : PlayerRopeTouchState
    {
        public PlayerRopeClimbState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
        }
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (isExitingState) return;
            player.SetVelocityX(0f);
            player.SetVelocityY(playerData.wallClimbVelocity * inputY);
            if (inputY == 0 && interactInput)
            {
                stateMachine.ChangeState(player.RopeGrabState);
            }
        }
    }
}

