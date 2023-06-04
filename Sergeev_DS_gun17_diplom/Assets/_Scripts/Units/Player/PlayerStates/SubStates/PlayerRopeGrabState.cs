using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Metroidvania.Player
{
    public class PlayerRopeGrabdState : PlayerRopeTouchState
    {
        private Vector2 _holdPosition;
        public PlayerRopeGrabdState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
        }
        public override void Enter()
        {
            base.Enter();
            _holdPosition = Player.transform.position;
            HoldPosition();
        }
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (IsExitingState) return;
            HoldPosition();
            if (InputY != 0)
            {
                StateMachine.ChangeState(Player.RopeClimbState);
            }
            else if (!InteractInput)
            {
                StateMachine.ChangeState(Player.InAirState);
            }
        }

        private void HoldPosition()
        {
            Player.transform.position = _holdPosition;
            Movement?.SetVelocityX(0f);
            Movement?.SetVelocityY(0f);
        }
    }
}

