using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Metroidvania.Enemy
{
    public class EnemyChargeState : EnemyState
    {
        protected bool isPlayerInMinAggroRange;
        protected bool isDetectingWall;
        protected bool isDetectingLedge;
        protected bool isChargeTimeOver;
        protected bool performCloseRangeAction;

        public EnemyChargeState(BaseEnemy enemy, EnemyStateMachine stateMachine, EnemyData enemyData, string animBoolName) : base(enemy, stateMachine, enemyData, animBoolName)
        {
        }
        public override void Enter()
        {
            base.Enter();
            enemy.SetVelocity(enemyData.chargeVelocity);
            isChargeTimeOver = false;
        }
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if(Time.time >= startTime + enemyData.chargeTime)
            {
                isChargeTimeOver = true;
            }
        }
        public override void DoChecks()
        {
            base.DoChecks();
            isPlayerInMinAggroRange = enemy.CheckPlayerInMinRange();
            isDetectingLedge = enemy.CheckLedge();
            isDetectingWall = enemy.CheckWall();
            performCloseRangeAction = enemy.CheckPlayerInCloseRangeAction();
        }
    }
}

