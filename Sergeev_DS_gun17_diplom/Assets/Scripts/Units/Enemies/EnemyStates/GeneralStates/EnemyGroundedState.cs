using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Metroidvania.Enemy
{
    public class EnemyGroundedState : EnemyState
    {
        protected bool isPlayerInMinAggroRange;
        public EnemyGroundedState(BaseEnemy enemy, EnemyStateMachine stateMachine, EnemyData enemyData, string animBoolName) : base(enemy, stateMachine, enemyData, animBoolName)
        {
        }
        public override void DoChecks()
        {
            base.DoChecks();
            isPlayerInMinAggroRange = enemy.CheckPlayerInMinRange();
        }
    }

}

