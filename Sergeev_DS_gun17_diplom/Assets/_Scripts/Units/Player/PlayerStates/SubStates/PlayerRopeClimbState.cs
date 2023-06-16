namespace Metroidvania.Player
{
    public class PlayerRopeClimbState : PlayerRopeTouchState
    {
        public PlayerRopeClimbState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
        }
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (IsExitingState) return;
            Movement?.SetVelocityX(0f);
            Movement?.SetVelocityY(PlayerData.wallClimbVelocity * InputY);
            Movement?.Flip();
            if (InputY == 0 && InteractInput)
            {
                StateMachine.ChangeState(Player.RopeGrabState);
            }
        }
    }
}

