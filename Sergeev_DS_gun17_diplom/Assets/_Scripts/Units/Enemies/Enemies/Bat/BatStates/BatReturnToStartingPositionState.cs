using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Metroidvania.Enemy
{
    public class BatReturnToStartingPositionState : EnemyReturnToStartPositionState
    {
        private readonly BatEnemy _batEnemy;
        private bool _isOnStartingPosition;
        private bool _isPlayerInMaxAggroRange;

        public BatReturnToStartingPositionState(BaseEnemy enemy, EnemyStateMachine stateMachine, EnemyData enemyData,
            string animBoolName, Vector2 startingPosition, BatEnemy batEnemy) : base(enemy, stateMachine, enemyData,
            animBoolName,
            startingPosition)
        {
            _batEnemy = batEnemy;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (_isPlayerInMaxAggroRange)
                StateMachine.ChangeState(_batEnemy.ChasingPlayerState);
            else if (_isOnStartingPosition)
                StateMachine.ChangeState(_batEnemy.IdleState);
        }

        protected override void DoChecks()
        {
            base.DoChecks();
            _isOnStartingPosition = _batEnemy.CheckOnStartingPosition();
            _isPlayerInMaxAggroRange = _batEnemy.CheckPlayerInMaxRange();
        }
    }
}