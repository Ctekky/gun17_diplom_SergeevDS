using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Metroidvania.BaseUnit;
namespace Metroidvania.Enemy
{
    public class EnemyDetectedPlayerState : EnemyState
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
        protected bool IsPlayerImMaxAggroRange;
        protected bool PerformLongRangeAction;
        protected bool PerformCloseRangeAction;
        public EnemyDetectedPlayerState(BaseEnemy enemy, EnemyStateMachine stateMachine, EnemyData enemyData, string animBoolName) : base(enemy, stateMachine, enemyData, animBoolName)
        {
        }
        public override void Enter()
        {
            base.Enter();
        }
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (IsExitingState) return;
        }
        public override void DoChecks()
        {
            base.DoChecks();
            IsPlayerInMinAggroRange = Enemy.CheckPlayerInMinRange();
            IsPlayerImMaxAggroRange = Enemy.CheckPlayerInMaxRange();
            PerformCloseRangeAction = Enemy.CheckPlayerInCloseRangeAction();
            PerformLongRangeAction = Enemy.CheckPlayerInLongRangeAction();
        }
    }
}

