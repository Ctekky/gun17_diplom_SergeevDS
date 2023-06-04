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
            if (IsExitingState) return;
            Movement?.SetVelocityX(0f);
            Movement?.SetVelocityY(PlayerData.wallClimbVelocity);
            if (InputY != 1)
            {
                StateMachine.ChangeState(Player.WallGrabState);
            }
        }
    }

}