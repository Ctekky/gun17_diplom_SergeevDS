using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Metroidvania.Enemy
{
    public class BoarIdleState : EnemyIdleState
    {
        private BoarEnemy boarEnemy;
        public BoarIdleState(BaseEnemy enemy, EnemyStateMachine stateMachine, EnemyData enemyData, string animBoolName, BoarEnemy boarEnemy) : base(enemy, stateMachine, enemyData, animBoolName)
        {
            this.boarEnemy = boarEnemy;
        }
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (isPlayerInMinAggroRange)
            {
                stateMachine.ChangeState(boarEnemy.DetectedPlayerState);
            }
            else if (Time.time >= startTime + idleTime) stateMachine.ChangeState(boarEnemy.MoveState);
        }
    }
}

