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
            Player.SetColliderHeight(PlayerData.colliderCrouchHeight);
        }
        public override void Exit()
        {
            base.Exit();
            Player.SetColliderHeight(PlayerData.colliderStandHeight);
        }
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (IsExitingState) return;
            if(InputX !=0 )
            {
                StateMachine.ChangeState(Player.CrouchMoveState);
            }
            else if(InputY != -1 && !IsHeadTouchingGround)
            {
                StateMachine.ChangeState(Player.IdleState);
            }
        }
    }

}
