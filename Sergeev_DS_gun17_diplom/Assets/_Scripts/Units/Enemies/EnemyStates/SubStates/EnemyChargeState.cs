using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Metroidvania.BaseUnit;

namespace Metroidvania.Enemy
{
    public class EnemyChargeState : EnemyState
    {
        protected Movement Movement
        {
            get => movement ?? unit.GetUnitComponent<Movement>(ref movement);
        }
        private CollisionChecks CollisionChecks
        {
            get => collisionChecks ?? unit.GetUnitComponent<CollisionChecks>(ref collisionChecks);
        }
        private Movement movement;
        private CollisionChecks collisionChecks;
        protected bool isPlayerInMinAggroRange;
        protected bool isDetectingWall;
        protected bool isDetectingLedge;
        protected bool isChargeTimeOver;
        protected bool performCloseRangeAction;

        public EnemyChargeState(BaseEnemy enemy, EnemyStateMachine stateMachine, EnemyData enemyData, string animBoolName) : base(enemy, stateMachine, enemyData, animBoolName)
        {
        }
        public override void Enter()
        {
            base.Enter();
            Movement?.SetVelocityX(enemyData.chargeVelocity * Movement.FacingDirection);
            isChargeTimeOver = false;
        }
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (isExitingState) return;
            Movement?.SetVelocityX(enemyData.chargeVelocity * Movement.FacingDirection);
            if (Time.time >= startTime + enemyData.chargeTime)
            {
                isChargeTimeOver = true;
            }
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
            performCloseRangeAction = enemy.CheckPlayerInCloseRangeAction();
        }
    }
}

