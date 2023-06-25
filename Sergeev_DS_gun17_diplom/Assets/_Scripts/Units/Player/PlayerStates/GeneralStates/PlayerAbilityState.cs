using Metroidvania.BaseUnit;

namespace Metroidvania.Player
{
    public class PlayerAbilityState : PlayerState
    {
        protected bool IsAbilityDone;
        protected Movement Movement => _movement ? _movement : Unit.GetUnitComponent<Movement>(ref _movement);

        protected CollisionChecks CollisionChecks => _collisionChecks
            ? _collisionChecks
            : Unit.GetUnitComponent<CollisionChecks>(ref _collisionChecks);

        private Movement _movement;
        private CollisionChecks _collisionChecks;
        private bool _isGrounded;
        protected int InputX;
        protected int InputY;

        protected PlayerAbilityState(Player player, PlayerStateMachine stateMachine, PlayerData playerData,
            string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
        }

        public override void DoChecks()
        {
            base.DoChecks();
            if (CollisionChecks) _isGrounded = CollisionChecks.Grounded;
        }
        public override void Enter()
        {
            base.Enter();
            IsAbilityDone = false;
        }
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            InputX = Player.InputHandler.NormalizedInputX;
            InputY = Player.InputHandler.NormalizedInputY;
            if (IsAbilityDone)
            {
                if (_isGrounded && Movement?.CurrentVelocity.y < 0.01f)
                {
                    StateMachine.ChangeState(Player.IdleState);
                }
                else
                {
                    StateMachine.ChangeState(Player.InAirState);
                }
            }
        }
    }
}