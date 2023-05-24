using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Metroidvania.Enemy
{
    public class EnemyMoveState : EnemyGroundedState
    {
        protected bool isDetectingWall;
        protected bool isDetectingLedge;
        
        public EnemyMoveState(BaseEnemy enemy, EnemyStateMachine stateMachine, EnemyData enemyData, string animBoolName) : base(enemy, stateMachine, enemyData, animBoolName)
        {
        }
        public override void Enter()
        {
            base.Enter();
            enemy.SetVelocity(enemyData.movementVelocity);

        }
        public override void DoChecks()
        {
            base.DoChecks();
            isDetectingLedge = enemy.CheckLedge();
            isDetectingWall = enemy.CheckWall();
            isPlayerInMinAggroRange = enemy.CheckPlayerInMinRange();
        }
    }
}


