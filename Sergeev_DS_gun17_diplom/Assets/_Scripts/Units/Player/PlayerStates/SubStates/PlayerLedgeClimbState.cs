using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Metroidvania.BaseUnit;

namespace Metroidvania.Player
{
    public class PlayerLedgeClimbState : PlayerState
    {
        protected Movement Movement
        {
            get => _movement ?? Unit.GetUnitComponent<Movement>(ref _movement);
        }
        private CollisionChecks CollisionChecks
        {
            get => _collisionChecks ?? Unit.GetUnitComponent<CollisionChecks>(ref _collisionChecks);
        }
        private Movement _movement;
        private CollisionChecks _collisionChecks;
        private Vector2 _detectedPos;
        private Vector2 _cornerPos;
        private Vector2 _startPos;
        private Vector2 _stopPos;
        private Vector2 _workVector;

        private bool _isHanging;
        private bool _isClimbing;
        private bool _isTouchHead;

        private int _inputX;
        private int _inputY;
        private bool _jumpInput;


        public PlayerLedgeClimbState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
        }

        public override void AnimationEndTrigger()
        {
            base.AnimationEndTrigger();
            Player.Animator.SetBool("climbLedge", false);
        }

        public override void AnimationTrigger()
        {
            base.AnimationTrigger();
            _isHanging = true;
            _isClimbing = false;
        }

        public override void Enter()
        {
            base.Enter();
            Movement?.SetVelocityZero();
            Player.transform.position = _detectedPos;
            _cornerPos = DetermineCornerPositon();
            _startPos.Set(_cornerPos.x - (Movement.FacingDirection * PlayerData.startOffset.x), _cornerPos.y - PlayerData.startOffset.y);
            _stopPos.Set(_cornerPos.x + (Movement.FacingDirection * PlayerData.stopOffset.x), _cornerPos.y + PlayerData.stopOffset.y);

            Player.transform.position = _startPos;
        }

        public override void Exit()
        {
            base.Exit();
            _isHanging = false;
            if (_isClimbing)
            {
                Player.transform.position = _stopPos;
                _isClimbing = false;
            }
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (IsAnimationEnd)
            {
                if (_isTouchHead)
                {
                    StateMachine.ChangeState(Player.CrouchIdleState);
                }
                else
                {
                    StateMachine.ChangeState(Player.IdleState);
                }
            }
            else
            {
                _inputX = Player.InputHandler.NormalizedInputX;
                _inputY = Player.InputHandler.NormalizedInputY;
                _jumpInput = Player.InputHandler.JumpInput;

                Movement?.SetVelocityZero();
                Player.transform.position = _startPos;

                if (_inputX == Movement?.FacingDirection && _isHanging && !_isClimbing)
                {
                    CheckForSpace();
                    _isClimbing = true;
                    Player.Animator.SetBool("climbLedge", true);
                }
                else if (_inputY == -1 && _isHanging && !_isClimbing)
                {
                    StateMachine.ChangeState(Player.InAirState);
                }
                else if (_jumpInput && !_isClimbing)
                {
                    Player.WallJumpState.WallJumpDirection(true);
                    StateMachine.ChangeState(Player.WallJumpState);
                }
            }
        }

        public void SetDetectedPosition(Vector2 pos) => _detectedPos = pos;
        private void CheckForSpace()
        {
            _isTouchHead = Physics2D.Raycast(_cornerPos + (Vector2.up * 0.015f) + (Vector2.right * Movement.FacingDirection * 0.015f),
                                            Vector2.up,
                                            PlayerData.colliderStandHeight,
                                            CollisionChecks.GroundLayer);
        }
        private Vector2 DetermineCornerPositon()
        {
            RaycastHit2D hitX = Physics2D.Raycast(CollisionChecks.WallCheck.position,
                                                  Vector2.right * Movement.FacingDirection,
                                                  CollisionChecks.WallCheckDistance,
                                                  CollisionChecks.GroundLayer);
            float distanceX = hitX.distance;
            _workVector.Set(distanceX * Movement.FacingDirection, 0f);
            RaycastHit2D hitY = Physics2D.Raycast(CollisionChecks.LedgeCheckHorizontal.position + (Vector3)(_workVector),
                                                  Vector2.down,
                                                  CollisionChecks.LedgeCheckHorizontal.position.y - CollisionChecks.WallCheck.position.y,
                                                  CollisionChecks.GroundLayer);
            float distanceY = hitY.distance;
            _workVector.Set(CollisionChecks.WallCheck.position.x + (distanceX * Movement.FacingDirection), CollisionChecks.LedgeCheckHorizontal.position.y - distanceY);
            return _workVector;
        }
    }
}

