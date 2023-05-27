using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Metroidvania.Enemy
{
    public class BoarMoveState : EnemyMoveState
    {
        private BoarEnemy boarEnemy;
        public BoarMoveState(BaseEnemy enemy, EnemyStateMachine stateMachine, EnemyData enemyData, string animBoolName, BoarEnemy boarEnemy) : base(enemy, stateMachine, enemyData, animBoolName)
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
            else if (isDetectingWall || !isDetectingLedge)
            {
                boarEnemy.IdleState.SetFlipAfterIdle(true);
                stateMachine.ChangeState(boarEnemy.IdleState);
            }
        }
    }
}

