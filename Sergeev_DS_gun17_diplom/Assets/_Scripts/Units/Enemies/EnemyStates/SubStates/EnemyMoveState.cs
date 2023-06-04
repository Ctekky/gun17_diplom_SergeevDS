using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Metroidvania.Enemy
{
    public class EnemyMoveState : EnemyGroundedState
    {
        protected bool IsDetectingWall;
        protected bool IsDetectingLedge;

        public EnemyMoveState(BaseEnemy enemy, EnemyStateMachine stateMachine, EnemyData enemyData, string animBoolName) : base(enemy, stateMachine, enemyData, animBoolName)
        {
        }
        public override void Enter()
        {
            base.Enter();
            Movement?.SetVelocityX(EnemyData.movementVelocity * Movement.FacingDirection);

        }
        public override void DoChecks()
        {
            base.DoChecks();
            if (CollisionChecks)
            {
                IsDetectingLedge = CollisionChecks.LedgeVertical;
                IsDetectingWall = CollisionChecks.WallFront;

            }
            IsPlayerInMinAggroRange = Enemy.CheckPlayerInMinRange();
        }
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (IsExitingState) return;
            Movement?.SetVelocityX(EnemyData.movementVelocity * Movement.FacingDirection);
        }
    }
}


