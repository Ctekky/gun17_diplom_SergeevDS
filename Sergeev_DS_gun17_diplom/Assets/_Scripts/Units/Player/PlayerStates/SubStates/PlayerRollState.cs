using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Metroidvania.Player
{
    public class PlayerRollState : PlayerAbilityState
    {
        public bool CanRoll { get; private set; }
        private Vector2 rollDirection;
        private bool isTouchHead;
        public PlayerRollState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
        }
        public override void DoChecks()
        {
            base.DoChecks();
        }

        public override void AnimationEndTrigger()
        {
            base.AnimationEndTrigger();
            player.SetPlayerLayer(playerData.standartPlayerLayer);
        }

        public override void AnimationTrigger()
        {
            base.AnimationTrigger();
            player.SetPlayerLayer(playerData.invinciblePlayerLayer);
        }

        public override void Enter()
        {
            base.Enter();
            CanRoll = false;
            player.InputHandler.UseRollInput();
            rollDirection = Vector2.right * Movement.FacingDirection;
            startTime = Time.time;
            player.SetColliderHeight(playerData.colliderCrouchHeight);
        }

        public override void Exit()
        {
            base.Exit();
            CanRoll = true;
            player.SetColliderHeight(playerData.colliderStandHeight);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (isExitingState) return;
            if (isAnimationEnd)
            {
                if (isTouchHead)
                {
                    isAbilityDone = true;
                    stateMachine.ChangeState(player.CrouchIdleState);
                }
                else
                {
                    isAbilityDone = true;
                    stateMachine.ChangeState(player.IdleState);
                }
            }
            else
            {
                player.RB.drag = playerData.drag;
                Movement.SetVelocity(playerData.rollVelocity, rollDirection);
                if (Time.time >= startTime + playerData.rollTime)
                {
                    player.RB.drag = 0f;

                }
                CheckForSpace();
            }

        }
        private void CheckForSpace()
        {
            isTouchHead = Physics2D.Raycast((Vector2)player.transform.position + (Vector2.up * 0.015f) + (Vector2.right
                * Movement.FacingDirection
                * 0.015f), Vector2.up, playerData.colliderCrouchHeight, CollisionChecks.GroundLayer);
        }
    }
}

