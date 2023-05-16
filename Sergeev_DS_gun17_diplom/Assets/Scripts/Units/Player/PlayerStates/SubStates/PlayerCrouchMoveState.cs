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
            player.SetColliderHeight(playerData.colliderCrouchHeight);
        }
        public override void Exit()
        {
            base.Exit();
            player.SetColliderHeight(playerData.colliderStandHeight);
        }
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (isExitingState) return;
            
            player.SetVelocityX(playerData.crouchMovementVelocity * player.FacingDirection);
            player.CheckFlip(inputX);
            if (inputX ==0 )
            {
                stateMachine.ChangeState(player.CrouchIdleState);
            }
            if(inputY != - 1 && !isHeadTouchingGround)
            {
                stateMachine.ChangeState(player.MoveState);
            }
        }
    }
}

