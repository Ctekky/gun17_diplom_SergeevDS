using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Metroidvania.Enemy
{
    public class BoarIdleState : EnemyIdleState
    {
        private BoarEnemy _boarEnemy;
        public BoarIdleState(BaseEnemy enemy, EnemyStateMachine stateMachine, EnemyData enemyData, string animBoolName, BoarEnemy boarEnemy) : base(enemy, stateMachine, enemyData, animBoolName)
        {
            this._boarEnemy = boarEnemy;
        }
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (IsPlayerInMinAggroRange)
            {
                StateMachine.ChangeState(_boarEnemy.DetectedPlayerState);
            }
            else if (Time.time >= StartTime + IdleTime) StateMachine.ChangeState(_boarEnemy.MoveState);
        }
    }
}

