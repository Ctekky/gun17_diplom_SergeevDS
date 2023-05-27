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
            get => movement ?? unit.GetUnitComponent<Movement>(ref movement);
        }
        private CollisionChecks CollisionChecks
        {
            get => collisionChecks ?? unit.GetUnitComponent<CollisionChecks>(ref collisionChecks);
        }
        private Movement movement;
        private CollisionChecks collisionChecks;
        protected bool isPlayerInMinAggroRange;
        protected bool isPlayerImMaxAggroRange;
        protected bool performLongRangeAction;
        protected bool performCloseRangeAction;
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
            if (isExitingState) return;
        }
        public override void DoChecks()
        {
            base.DoChecks();
            isPlayerInMinAggroRange = enemy.CheckPlayerInMinRange();
            isPlayerImMaxAggroRange = enemy.CheckPlayerInMaxRange();
            performCloseRangeAction = enemy.CheckPlayerInCloseRangeAction();
            performLongRangeAction = enemy.CheckPlayerInLongRangeAction();
        }
    }
}

