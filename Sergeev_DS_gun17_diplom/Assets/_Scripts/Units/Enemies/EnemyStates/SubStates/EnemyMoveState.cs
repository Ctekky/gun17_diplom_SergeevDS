using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Metroidvania.Enemy
{
    public class EnemyMoveState : EnemyGroundedState
    {
        protected bool isDetectingWall;
        protected bool isDetectingLedge;

        public EnemyMoveState(BaseEnemy enemy, EnemyStateMachine stateMachine, EnemyData enemyData, string animBoolName) : base(enemy, stateMachine, enemyData, animBoolName)
        {
        }
        public override void Enter()
        {
            base.Enter();
            Movement?.SetVelocityX(enemyData.movementVelocity * Movement.FacingDirection);

        }
        public override void DoChecks()
        {
            base.DoChecks();
            if (CollisionChecks)
            {
                isDetectingLedge = CollisionChecks.LedgeVertical;
                isDetectingWall = CollisionChecks.WallFront;

            }
            isPlayerInMinAggroRange = enemy.CheckPlayerInMinRange();
        }
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (isExitingState) return;
            Movement?.SetVelocityX(enemyData.movementVelocity * Movement.FacingDirection);
        }
    }
}


