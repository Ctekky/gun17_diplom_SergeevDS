using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Metroidvania.Player
{
    public class PlayerCrouchMoveState : PlayerGroundedState
    {
        public PlayerCrouchMoveState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
        }
        public override void Enter()
        {
            base.Enter();
            Player.SetColliderHeight(PlayerData.colliderCrouchHeight);
            Player.audioManager.PlaySFX((int)SFXSlots.FootStep);
        }
        public override void Exit()
        {
            base.Exit();
            Player.SetColliderHeight(PlayerData.colliderStandHeight);
            Player.audioManager.StopSFX((int)SFXSlots.FootStep);
        }
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (IsExitingState) return;
            
            Movement?.SetVelocityX(PlayerData.crouchMovementVelocity * Movement.FacingDirection);
            Movement?.CheckFlip(InputX);
            if (InputX ==0 )
            {
                StateMachine.ChangeState(Player.CrouchIdleState);
            }
            if(InputY != - 1 && !IsHeadTouchingGround)
            {
                StateMachine.ChangeState(Player.MoveState);
            }
        }
    }
}

