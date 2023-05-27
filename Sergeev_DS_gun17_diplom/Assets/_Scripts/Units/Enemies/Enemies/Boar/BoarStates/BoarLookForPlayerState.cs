using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Metroidvania.Enemy
{
    public class BoarLookForPlayerState : EnemyLookForPlayerState
    {
        private BoarEnemy boarEnemy;
        public BoarLookForPlayerState(BaseEnemy enemy, EnemyStateMachine stateMachine, EnemyData enemyData, string animBoolName, BoarEnemy boarEnemy) : base(enemy, stateMachine, enemyData, animBoolName)
        {
            this.boarEnemy = boarEnemy;
        }
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if(isPlayerInMinAggroRange)
            {
                stateMachine.ChangeState(boarEnemy.DetectedPlayerState);
            }
            else if(isAllTurnsDone)
            {
                stateMachine.ChangeState(boarEnemy.MoveState);
            }
        }
    }
}

