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
            if (IsExitingState) return;

            Movement?.SetVelocityX(0f);
            Movement.SetVelocityY(-PlayerData.wallSlideVelocity);
            if (InteractInput && InputY == 0)
            {
                StateMachine.ChangeState(Player.WallGrabState);
            }
        }
    }
}

