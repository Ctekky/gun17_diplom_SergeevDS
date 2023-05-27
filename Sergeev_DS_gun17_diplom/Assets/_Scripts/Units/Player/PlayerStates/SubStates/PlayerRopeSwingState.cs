using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Metroidvania.Player
{
    public class PlayerRopeSwingState : PlayerRopeTouchState
    {
        private Vector2 holdPosition;
        public PlayerRopeSwingState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
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
            if (inputX != 0 && interactInput)
            {
                player.PlayerMoveRope();
            }
            if (inputX == 0 && interactInput)
            {
                stateMachine.ChangeState(player.RopeGrabState);
            }
        }
        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }
        private void HoldPosition()
        {
            player.transform.position = player.CurrentRope.position;
            player.transform.rotation = player.CurrentRope.rotation;
            Movement?.SetVelocityX(0f);
            Movement?.SetVelocityY(0f);
        }

    }
}

