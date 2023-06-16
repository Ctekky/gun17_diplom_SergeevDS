using Metroidvania.BaseUnit;

namespace Metroidvania.Player
{
    public class PlayerRopeTouchState : PlayerState
    {
        private bool _isGrounded;
        private bool _isTouchingWall;
        private bool _isTouchingRope;
        protected bool InteractInput;
        protected int InputX;
        protected int InputY;
        private bool _jumpInput;
        protected Movement Movement => _movement ? _movement : Unit.GetUnitComponent<Movement>(ref _movement);

        private CollisionChecks CollisionChecks => _collisionChecks
            ? _collisionChecks
            : Unit.GetUnitComponent<CollisionChecks>(ref _collisionChecks);

        private Movement _movement;
        private CollisionChecks _collisionChecks;

        protected PlayerRopeTouchState(Player player, PlayerStateMachine stateMachine, PlayerData playerData,
            string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
        }
        public override void DoChecks()
        {
            base.DoChecks();
            if (CollisionChecks != null)
            {
                _isGrounded = CollisionChecks.Grounded;
                _isTouchingWall = CollisionChecks.WallFront;
                _isTouchingRope = Player.IsTouchingRope;
            }
        }
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            InputX = Player.InputHandler.NormalizedInputX;
            InputY = Player.InputHandler.NormalizedInputY;
            InteractInput = Player.InputHandler.InteractInput;
            _jumpInput = Player.InputHandler.JumpInput;
            if (_jumpInput)
            {
                Player.WallJumpState.WallJumpDirection(_isTouchingWall);
                StateMachine.ChangeState(Player.WallJumpState);
            }
            else if (_isGrounded && !InteractInput)
            {
                StateMachine.ChangeState(Player.IdleState);
            }
            else if (!_isTouchingRope || (InputX != Movement?.FacingDirection && !InteractInput))
            {
                StateMachine.ChangeState(Player.InAirState);
            }
        }
    }
}