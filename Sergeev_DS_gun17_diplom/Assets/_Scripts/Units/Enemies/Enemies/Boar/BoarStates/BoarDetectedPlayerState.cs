using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Metroidvania.Enemy
{
    public class BoarDetectedPlayerState : EnemyDetectedPlayerState
    {
        private BoarEnemy _boarEnemy;
        public BoarDetectedPlayerState(BaseEnemy enemy, EnemyStateMachine stateMachine, EnemyData enemyData, string animBoolName, BoarEnemy boarEnemy) : base(enemy, stateMachine, enemyData, animBoolName)
        {
            this._boarEnemy = boarEnemy;
        }
        public override void Enter()
        {
            base.Enter();
            Movement?.SetVelocityZero();
        }
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if(PerformCloseRangeAction)
            {
                StateMachine.ChangeState(_boarEnemy.MeleeAttackState);
            }
            else if (PerformLongRangeAction)
            {
                if (Time.time >= StartTime + EnemyData.actionTime)
                {
                     StateMachine.ChangeState(_boarEnemy.ChargeState);
                }
            }
            else if(!IsPlayerImMaxAggroRange)
            {
                StateMachine.ChangeState(_boarEnemy.LookForPlayerState);
            }
        }
    }
}

