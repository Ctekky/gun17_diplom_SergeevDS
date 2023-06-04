using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Metroidvania.Enemy
{
    public class BoarDeadState : EnemyDeadState
    {
        private BoarEnemy _boarEnemy;
        public BoarDeadState(BaseEnemy enemy, EnemyStateMachine stateMachine, EnemyData enemyData, string animBoolName, BoarEnemy boarEnemy) : base(enemy, stateMachine, enemyData, animBoolName)
        {
            _boarEnemy = boarEnemy;
        }
    }

}
