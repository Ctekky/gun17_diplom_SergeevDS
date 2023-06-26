using Metroidvania.BaseUnit;
using UnityEngine;

namespace Metroidvania.Enemy
{
    public class EnemyReturnToStartPositionState : EnemyState
    {
        private Movement Movement => _movement ? _movement : Unit.GetUnitComponent<Movement>(ref _movement);

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
        private readonly Vector2 _startingPosition;

        protected EnemyReturnToStartPositionState(BaseEnemy enemy, EnemyStateMachine stateMachine, EnemyData enemyData,
            string animBoolName, Vector2 startingPosition) : base(enemy, stateMachine, enemyData, animBoolName)
        {
            _startingPosition = startingPosition;
        }

        public override void Enter()
        {
            base.Enter();
            Movement?.SetVelocityZero();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (IsExitingState) return;
            Movement?.SetVelocityToTarget(_startingPosition, EnemyData.movementVelocity);
            
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
        }
    }
}