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
            get => _movement ?? Unit.GetUnitComponent<Movement>(ref _movement);
        }
        private CollisionChecks CollisionChecks
        {
            get => _collisionChecks ?? Unit.GetUnitComponent<CollisionChecks>(ref _collisionChecks);
        }
        private Movement _movement;
        private CollisionChecks _collisionChecks;
        protected bool IsPlayerInMinAggroRange;
        protected bool IsDetectingWall;
        protected bool IsDetectingLedge;
        protected bool IsChargeTimeOver;
        protected bool PerformCloseRangeAction;

        public EnemyChargeState(BaseEnemy enemy, EnemyStateMachine stateMachine, EnemyData enemyData, string animBoolName) : base(enemy, stateMachine, enemyData, animBoolName)
        {
        }
        public override void Enter()
        {
            base.Enter();
            Movement?.SetVelocityX(EnemyData.chargeVelocity * Movement.FacingDirection);
            IsChargeTimeOver = false;
        }
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (IsExitingState) return;
            Movement?.SetVelocityX(EnemyData.chargeVelocity * Movement.FacingDirection);
            if (Time.time >= StartTime + EnemyData.chargeTime)
            {
                IsChargeTimeOver = true;
            }
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
            PerformCloseRangeAction = Enemy.CheckPlayerInCloseRangeAction();
        }
    }
}

