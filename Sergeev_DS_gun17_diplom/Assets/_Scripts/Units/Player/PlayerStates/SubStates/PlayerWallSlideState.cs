using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Metroidvania.Player
{
    public class PlayerWallSlideState : PlayerWallTouchState
    {
        public PlayerWallSlideState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (isExitingState) return;

            Movement?.SetVelocityX(0f);
            Movement.SetVelocityY(-playerData.wallSlideVelocity);
            if (interactInput && inputY == 0)
            {
                stateMachine.ChangeState(player.WallGrabState);
            }
        }
    }
}

