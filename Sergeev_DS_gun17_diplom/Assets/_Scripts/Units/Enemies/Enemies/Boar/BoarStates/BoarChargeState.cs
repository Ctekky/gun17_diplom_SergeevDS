using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Metroidvania.Enemy
{
    public class BoarChargeState : EnemyChargeState
    {
        private BoarEnemy _boarEnemy;
        public BoarChargeState(BaseEnemy enemy, EnemyStateMachine stateMachine, EnemyData enemyData, string animBoolName, BoarEnemy boarEnemy) : base(enemy, stateMachine, enemyData, animBoolName)
        {
            this._boarEnemy = boarEnemy;
        }
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (IsDetectingWall || !IsDetectingLedge)
            {
                StateMachine.ChangeState(_boarEnemy.LookForPlayerState);
            }
            else if(IsChargeTimeOver || PerformCloseRangeAction)
            {
                if(PerformCloseRangeAction)
                {
                    StateMachine.ChangeState(_boarEnemy.MeleeAttackState);
                }    
                else if(IsPlayerInMinAggroRange)
                {
                    StateMachine.ChangeState(_boarEnemy.DetectedPlayerState);
                }
            }
        }
    }
}

