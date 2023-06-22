using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Metroidvania.Player
{
    public class PlayerRollState : PlayerAbilityState
    {
        public bool CanRoll { get; private set; }
        private Vector2 _rollDirection;
        private bool _isTouchHead;
        public PlayerRollState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
        }
        public override void AnimationEndTrigger()
        {
            base.AnimationEndTrigger();
            Player.SetPlayerLayer(PlayerData.standartPlayerLayer);
        }

        public override void AnimationTrigger()
        {
            base.AnimationTrigger();
            Player.SetPlayerLayer(PlayerData.invinciblePlayerLayer);
        }
        public override void Enter()
        {
            base.Enter();
            CanRoll = false;
            Player.InputHandler.UseRollInput();
            _rollDirection = Vector2.right * Movement.FacingDirection;
            StartTime = Time.time;
            Player.SetColliderHeight(PlayerData.colliderCrouchHeight);
        }

        public override void Exit()
        {
            base.Exit();
            CanRoll = true;
            Player.SetColliderHeight(PlayerData.colliderStandHeight);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (IsExitingState) return;
            if (IsAnimationEnd)
            {
                if (_isTouchHead)
                {
                    IsAbilityDone = true;
                    StateMachine.ChangeState(Player.CrouchIdleState);
                }
                else
                {
                    IsAbilityDone = true;
                    StateMachine.ChangeState(Player.IdleState);
                }
            }
            else
            {
                Player.Rb.drag = PlayerData.drag;
                Movement.SetVelocity(PlayerData.rollVelocity, _rollDirection);
                if (Time.time >= StartTime + PlayerData.rollTime)
                {
                    Player.Rb.drag = 0f;

                }
                CheckForSpace();
            }

        }
        private void CheckForSpace()
        {
            _isTouchHead = Physics2D.Raycast((Vector2)Player.transform.position + (Vector2.up * 0.015f) + (Vector2.right
                * Movement.FacingDirection
                * 0.015f), Vector2.up, PlayerData.colliderCrouchHeight, CollisionChecks.GroundLayer);
        }
    }
}

