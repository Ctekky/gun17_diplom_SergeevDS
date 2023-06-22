using UnityEngine;

namespace Metroidvania.Player
{
    public class PlayerRopeGrabState : PlayerRopeTouchState
    {
        private Vector2 _holdPosition;
        public PlayerRopeGrabState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
        }
        public override void Enter()
        {
            base.Enter();
            CurrentRope = Player.CurrentRope;
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
            else if (JumpInput)
            {
                StateMachine.ChangeState(Player.InAirState);
            }
        }
        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            Movement?.CheckFlip(InputX);
        }
        private void HoldPosition()
        {
            var ropeTransform = CurrentRope.transform;
            var playerTransform = Player.transform;
            playerTransform.position = ropeTransform.position;
            Movement?.SetVelocityZero();
        }
    }
}

