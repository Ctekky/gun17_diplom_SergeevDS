using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Metroidvania.Enemy
{
    public class EnemyDetectedPlayerState : EnemyState
    {
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

