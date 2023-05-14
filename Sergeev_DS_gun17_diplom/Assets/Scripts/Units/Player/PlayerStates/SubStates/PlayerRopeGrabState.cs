using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Metroidvania.Player
{
    public class PlayerRopeGrabdState : PlayerRopeTouchState
    {
        private Vector2 holdPosition;
        public PlayerRopeGrabdState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
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
            if (inputY != 0)
            {
                stateMachine.ChangeState(player.RopeClimbState);
            }
            else if (!interactInput)
            {
                stateMachine.ChangeState(player.InAirState);
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

