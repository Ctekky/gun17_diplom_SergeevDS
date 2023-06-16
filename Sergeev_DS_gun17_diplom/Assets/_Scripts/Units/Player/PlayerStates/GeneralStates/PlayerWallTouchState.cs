using Metroidvania.BaseUnit;

namespace Metroidvania.Player
{
    public class PlayerWallTouchState : PlayerState
    {
        private bool _isGrounded;
        private bool _isTouchingWall;
        protected bool InteractInput;
        private int _inputX;
        protected int InputY;
        private bool _jumpInput;
        private bool _isTouchingLedge;
        protected Movement Movement => _movement ? _movement : Unit.GetUnitComponent<Movement>(ref _movement);

        private CollisionChecks CollisionChecks => _collisionChecks
            ? _collisionChecks
            : Unit.GetUnitComponent<CollisionChecks>(ref _collisionChecks);

        private Movement _movement;
        private CollisionChecks _collisionChecks;


        protected PlayerWallTouchState(Player player, PlayerStateMachine stateMachine, PlayerData playerData,
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
                _isTouchingLedge = CollisionChecks.LedgeHorizontal;
            }

            if (_isTouchingWall && !_isTouchingLedge)
            {
                Player.LedgeClimbState.SetDetectedPosition(Player.transform.position);
            }
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            _inputX = Player.InputHandler.NormalizedInputX;
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
            else if (!_isTouchingWall || (_inputX != Movement?.FacingDirection && !InteractInput))
            {
                StateMachine.ChangeState(Player.InAirState);
            }
            else if (_isTouchingWall && !_isTouchingLedge)
            {
                StateMachine.ChangeState(Player.LedgeClimbState);
            }
        }
    }
}