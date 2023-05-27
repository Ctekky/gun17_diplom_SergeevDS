using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Metroidvania.Player
{
    public class PlayerCrouchIdleState : PlayerGroundedState
    {
        public PlayerCrouchIdleState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
        }
        public override void Enter()
        {
            base.Enter();
            Movement?.SetVelocityZero();
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
            if(inputX !=0 )
            {
                stateMachine.ChangeState(player.CrouchMoveState);
            }
            else if(inputY != -1 && !isHeadTouchingGround)
            {
                stateMachine.ChangeState(player.IdleState);
            }
        }
    }

}
