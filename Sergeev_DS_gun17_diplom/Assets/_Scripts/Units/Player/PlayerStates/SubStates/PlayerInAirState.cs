using UnityEngine;
using Metroidvania.BaseUnit;

namespace Metroidvania.Player
{
    public class PlayerInAirState : PlayerState
    {
        private Movement Movement
        {
            get => _movement ? _movement : Unit.GetUnitComponent<Movement>(ref _movement);
        }
        private CollisionChecks CollisionChecks
        {
            get => _collisionChecks ? _collisionChecks : Unit.GetUnitComponent<CollisionChecks>(ref _collisionChecks);
        }
        private Movement _movement;
        private CollisionChecks _collisionChecks;
        private int _inputX;
        private bool _isGrounded;
        private bool _isTouchingWall;
        private bool _isTouchingWallBack;
        private bool _oldIsTouchingWall;
        private bool _oldIsTouchingWallBack;
        private bool _jumpInput;
        private bool _jumpInputStop;
        private bool _interactInput;
        private bool _lastMomentJump;
        private bool _lastMomentWallJump;
        private bool _isJumping;
        private bool _isTouchingRope;
        private bool _isTouchingLedge;

        private float _startLastMomentWallJumpTime;

        public PlayerInAirState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
        }

        public override void DoChecks()
        {
            base.DoChecks();
            _oldIsTouchingWall = _isTouchingWall;
            _oldIsTouchingWallBack = _isTouchingWallBack;

            if (CollisionChecks)
            {
                _isGrounded = CollisionChecks.Grounded;
                _isTouchingWall = CollisionChecks.WallFront;
                _isTouchingWallBack = CollisionChecks.WallBack;
                _isTouchingRope = Player.IsTouchingRope;
                _isTouchingLedge = CollisionChecks.LedgeHorizontal;

            }
            if (_isTouchingWall && !_isTouchingLedge)
            {
                Player.LedgeClimbState.SetDetectedPosition(Player.transform.position);
            }

            if (!_lastMomentWallJump && !_isTouchingWall && !_isTouchingWallBack && (_oldIsTouchingWall || _oldIsTouchingWallBack))
            {
                StartLastMomentWallJumpTimer();
            }
        }
        public override void Exit()
        {
            base.Exit();
            _oldIsTouchingWall = false;
            _oldIsTouchingWallBack = false;
            _isTouchingWall = false;
            _isTouchingWallBack = false;

        }
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            CheckLastMomentJump();
            CheckLastMomentWallJump();

            _inputX = Player.InputHandler.NormalizedInputX;
            _jumpInput = Player.InputHandler.JumpInput;
            _jumpInputStop = Player.InputHandler.JumpInputStop;
            _interactInput = Player.InputHandler.InteractInput;

            CheckJumpMultiplier();
            if (Player.InputHandler.AttackInputs[(int)CombatInputs.Primary])
            {
                StateMachine.ChangeState(Player.PrimaryAttackState);
            }
            else if (Player.InputHandler.AttackInputs[(int)CombatInputs.Secondary])
            {
                StateMachine.ChangeState(Player.SecondaryAttackState);
            }
            else if (_isGrounded && Movement?.CurrentVelocity.y < 0.01f)
            {
                StateMachine.ChangeState(Player.LandState);
            }
            else if (_isTouchingWall && !_isTouchingLedge && !_isGrounded)
            {
                StateMachine.ChangeState(Player.LedgeClimbState);
            }
            else if (_jumpInput && (_isTouchingWall || _isTouchingWallBack || _lastMomentWallJump))
            {
                StopLastMomentWallJumpTimer();
                _isTouchingWall = CollisionChecks.WallFront;
                Player.WallJumpState.WallJumpDirection(_isTouchingWall);
                StateMachine.ChangeState(Player.WallJumpState);
            }
            else if (_jumpInput && Player.JumpState.CanJump())
            {
                StateMachine.ChangeState(Player.JumpState);
            }
            else if (_isTouchingRope && _interactInput)
            {
                StateMachine.ChangeState(Player.RopeGrabState);
            }
            else if (_isTouchingWall && _interactInput && _isTouchingLedge)
            {
                StateMachine.ChangeState(Player.WallGrabState);
            }
            else if (_isTouchingWall && _inputX == Movement?.FacingDirection && Movement?.CurrentVelocity.y <= 0f)
            {
                StateMachine.ChangeState(Player.WallSlideState);
            }
            else
            {
                Movement?.CheckFlip(_inputX);
                Movement?.SetVelocityX(PlayerData.movementVelocity * _inputX);
                Player.Animator.SetFloat("velocityY", Movement.CurrentVelocity.y);
            }
        }

        private void CheckJumpMultiplier()
        {
            if (_isJumping)
            {
                if (_jumpInputStop)
                {
                    Movement?.SetVelocityY(Movement.CurrentVelocity.y * PlayerData.jumpHeighMultiplier);
                    _isJumping = false;
                }
                else if (Movement?.CurrentVelocity.y <= 0f)
                {
                    _isJumping = false;
                }
            }
        }
        private void CheckLastMomentJump()
        {
            if (_lastMomentJump && Time.time > StartTime + PlayerData.lastMomentJumpTime)
            {
                _lastMomentJump = false;
                Player.JumpState.DecreaseJumps();
            }
        }
        private void CheckLastMomentWallJump()
        {
            if (_lastMomentWallJump && Time.time > _startLastMomentWallJumpTime + PlayerData.lastMomentJumpTime)
            {
                _lastMomentWallJump = false;
            }
        }
        public void StartLastMomentJumpTimer() => _lastMomentJump = true;
        public void SetIsJumping() => _isJumping = true;
        private void StartLastMomentWallJumpTimer()
        {
            _lastMomentWallJump = true;
            _startLastMomentWallJumpTime = Time.time;
        }
        private void StopLastMomentWallJumpTimer() => _lastMomentWallJump = false;

    }
}

