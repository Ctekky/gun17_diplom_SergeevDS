using UnityEngine;
using Metroidvania.BaseUnit;

namespace Metroidvania.Enemy
{
    public class EnemyChasingPlayerState : EnemyState
    {
        protected Movement Movement => _movement ? _movement : Unit.GetUnitComponent<Movement>(ref _movement);

        private CollisionChecks CollisionChecks => _collisionChecks
            ? _collisionChecks
            : Unit.GetUnitComponent<CollisionChecks>(ref _collisionChecks);

        private Movement _movement;
        private CollisionChecks _collisionChecks;
        protected bool IsPlayerInMinAggroRange;
        protected bool IsPlayerInMaxAggroRange;
        protected bool IsDetectingWall;
        protected bool IsDetectingLedge;
        protected bool IsChargeTimeOver;
        protected bool PerformCloseRangeAction;

        protected EnemyChasingPlayerState(BaseEnemy enemy, EnemyStateMachine stateMachine, EnemyData enemyData,
            string animBoolName) : base(enemy, stateMachine, enemyData, animBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();
            IsChargeTimeOver = false;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (IsExitingState) return;
            if (Time.time >= StartTime + EnemyData.chargeTime)
            {
                IsChargeTimeOver = true;
            }
        }

        protected override void DoChecks()
        {
            base.DoChecks();
            if (CollisionChecks)
            {
                IsDetectingLedge = CollisionChecks.LedgeVertical;
                IsDetectingWall = CollisionChecks.WallFront;
            }
            IsPlayerInMinAggroRange = Enemy.CheckPlayerInMinRange();
            PerformCloseRangeAction = Enemy.CheckPlayerInCloseRangeAction();
            IsPlayerInMaxAggroRange = Enemy.CheckPlayerInMinRange();
        }
    }
}