namespace Metroidvania.Player
{
    public class PlayerIdleState : PlayerGroundedState
    {
        public PlayerIdleState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
        }
        public override void Enter()
        {
            base.Enter();
            Movement?.SetVelocityZero();
        }
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (IsExitingState) return;
            if (InputX != 0)
            {
                StateMachine.ChangeState(Player.MoveState);
            }
            else if(InputY == -1)
            {
                StateMachine.ChangeState(Player.CrouchIdleState);
            }
        }
    }
}

