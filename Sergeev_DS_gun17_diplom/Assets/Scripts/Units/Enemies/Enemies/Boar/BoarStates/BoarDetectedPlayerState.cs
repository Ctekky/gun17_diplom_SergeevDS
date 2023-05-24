using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Metroidvania.Enemy
{
    public class BoarDetectedPlayerState : EnemyDetectedPlayerState
    {
        private BoarEnemy boarEnemy;
        public BoarDetectedPlayerState(BaseEnemy enemy, EnemyStateMachine stateMachine, EnemyData enemyData, string animBoolName, BoarEnemy boarEnemy) : base(enemy, stateMachine, enemyData, animBoolName)
        {
            this.boarEnemy = boarEnemy;
        }
        public override void Enter()
        {
            base.Enter();
            boarEnemy.SetVelocityZero();
        }
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if(performCloseRangeAction)
            {
                stateMachine.ChangeState(boarEnemy.MeleeAttackState);
            }
            else if (performLongRangeAction)
            {
                if (Time.time >= startTime + enemyData.actionTime) stateMachine.ChangeState(boarEnemy.ChargeState);
            }
            else if(!isPlayerImMaxAggroRange)
            {
                stateMachine.ChangeState(boarEnemy.LookForPlayerState);
            }
        }
    }
}

