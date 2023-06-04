using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Metroidvania.Enemy
{
    public class BoarMoveState : EnemyMoveState
    {
        private BoarEnemy _boarEnemy;
        public BoarMoveState(BaseEnemy enemy, EnemyStateMachine stateMachine, EnemyData enemyData, string animBoolName, BoarEnemy boarEnemy) : base(enemy, stateMachine, enemyData, animBoolName)
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
            else if (IsDetectingWall || !IsDetectingLedge)
            {
                _boarEnemy.IdleState.SetFlipAfterIdle(true);
                StateMachine.ChangeState(_boarEnemy.IdleState);
            }
        }
    }
}

