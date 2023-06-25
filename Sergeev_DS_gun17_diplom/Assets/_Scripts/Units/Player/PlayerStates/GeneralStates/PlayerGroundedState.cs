using Metroidvania.BaseUnit;

namespace Metroidvania.Player
{
    public class PlayerGroundedState : PlayerState
    {
        protected int InputX;
        protected int InputY;
        protected bool IsHeadTouchingGround;
        protected Movement Movement => _movement ? _movement : Unit.GetUnitComponent<Movement>(ref _movement);

        private CollisionChecks CollisionChecks => _collisionChecks
            ? _collisionChecks
            : Unit.GetUnitComponent<CollisionChecks>(ref _collisionChecks);

        private Movement _movement;
        private CollisionChecks _collisionChecks;
        private bool _jumpInput;
        private bool _interactInput;
        private bool _isGrounded;
        private bool _isTouchingWall;
        private bool _isTouchingLedge;
        private bool _rollInput;

        protected PlayerGroundedState(
            Player player,
            PlayerStateMachine stateMachine,
            PlayerData playerData,
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
                IsHeadTouchingGround = CollisionChecks.Ceiling;
                _isTouchingLedge = CollisionChecks.LedgeHorizontal;
            }
        }

        public override void Enter()
        {
            base.Enter();
            Player.JumpState.ResetJumps();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            InputX = Player.InputHandler.NormalizedInputX;
            InputY = Player.InputHandler.NormalizedInputY;
            _jumpInput = Player.InputHandler.JumpInput;
            _rollInput = Player.InputHandler.RollInput;
            _interactInput = Player.InputHandler.InteractInput;

            if (Player.InputHandler.AttackInputs[(int)CombatInputs.Primary] && !IsHeadTouchingGround)
            {
                if (Player.CurrentWeaponEquip == WeaponType.Bow && !Player.CanShoot())
                    StateMachine.ChangeState(Player.IdleState);
                else
                    StateMachine.ChangeState(Player.PrimaryAttackState);
            }
            else if (Player.InputHandler.SecondaryAttackStarted && !IsHeadTouchingGround &&
                     Player.CurrentWeaponEquip == WeaponType.Bow && InputX == 0 && InputY == 0)
            {
                if(!Player.CanShoot()) StateMachine.ChangeState(Player.IdleState);
                else StateMachine.ChangeState(Player.AimState);
            }
            else if (_jumpInput && Player.JumpState.CanJump() && !IsHeadTouchingGround)
            {
                StateMachine.ChangeState(Player.JumpState);
            }
            else if (!_isGrounded)
            {
                Player.InAirState.StartLastMomentJumpTimer();
                StateMachine.ChangeState(Player.InAirState);
            }
            else if (_isTouchingWall && _interactInput && _isTouchingLedge)
            {
                StateMachine.ChangeState(Player.WallGrabState);
            }
            else if (_rollInput)
            {
                StateMachine.ChangeState(Player.RollState);
            }
        }
    }
}