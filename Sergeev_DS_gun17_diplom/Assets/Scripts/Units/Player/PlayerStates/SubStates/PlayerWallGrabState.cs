using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Metroidvania.Player
{
    public class PlayerWallGrabState : PlayerWallTouchState
    {
        private Vector2 holdPosition;
        public PlayerWallGrabState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
        }
        public override void Enter()
        {
            base.Enter();
            holdPosition = player.transform.position;
            HoldPosition();
        }
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (isExitingState) return;
            HoldPosition();
            //отключаю ползанье по стенам (т.к. эта возможность только для веревки
            /*
            if (inputY > 0)
            {
                stateMachine.ChangeState(player.WallClimbState);
            }
            */
            if (inputY < 0 || !interactInput)
            {
                stateMachine.ChangeState(player.WallSlideState);
            }
        }

        private void HoldPosition()
        {
            player.transform.position = holdPosition;
            player.SetVelocityX(0f);
            player.SetVelocityY(0f);
        }
    }

}