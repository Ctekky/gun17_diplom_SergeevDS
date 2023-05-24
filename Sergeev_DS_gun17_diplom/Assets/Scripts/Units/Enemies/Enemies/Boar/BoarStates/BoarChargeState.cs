using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Metroidvania.Enemy
{
    public class BoarChargeState : EnemyChargeState
    {
        private BoarEnemy boarEnemy;
        public BoarChargeState(BaseEnemy enemy, EnemyStateMachine stateMachine, EnemyData enemyData, string animBoolName, BoarEnemy boarEnemy) : base(enemy, stateMachine, enemyData, animBoolName)
        {
            this.boarEnemy = boarEnemy;
        }
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if(isDetectingWall || !isDetectingLedge)
            {
                stateMachine.ChangeState(boarEnemy.LookForPlayerState);
            }
            else if(isChargeTimeOver || performCloseRangeAction)
            {
                if(performCloseRangeAction)
                {
                    stateMachine.ChangeState(boarEnemy.MeleeAttackState);
                }    
                else if(isPlayerInMinAggroRange)
                {
                    stateMachine.ChangeState(boarEnemy.DetectedPlayerState);
                }
            }
        }
    }
}

