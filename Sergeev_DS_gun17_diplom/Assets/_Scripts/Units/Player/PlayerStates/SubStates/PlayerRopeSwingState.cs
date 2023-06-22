using UnityEngine;

namespace Metroidvania.Player
{
    public class PlayerRopeSwingState : PlayerRopeTouchState
    {
        private Vector2 _holdPosition;
        public PlayerRopeSwingState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
        }
        public override void Enter()
        {
            base.Enter();
            _holdPosition = Player.transform.position;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (IsExitingState) return;
            if (InputX != 0)
            {
                Player.PlayerMoveRope();
            }

            if (InputX == 0 && InteractInput)
            {
                StateMachine.ChangeState(Player.RopeGrabState);
            }
        }

        private void HoldPosition()
        {
            Player.transform.position = Player.CurrentRope.position;
            Player.transform.rotation = Player.CurrentRope.rotation;
        }

    }
}

