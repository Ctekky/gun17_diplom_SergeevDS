using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Metroidvania.Player
{
    public class PlayerWallGrabState : PlayerWallTouchState
    {
        private Vector2 _holdPosition;
        public PlayerWallGrabState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
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

            if (InputY > 0)
            {
                //stateMachine.ChangeState(player.WallClimbState);
            }
            if (InputY < 0 || !InteractInput)
            {
                StateMachine.ChangeState(Player.WallSlideState);
            }
        }
        private void HoldPosition()
        {
            Player.transform.position = _holdPosition;
            Movement?.SetVelocityZero();
        }
    }

}