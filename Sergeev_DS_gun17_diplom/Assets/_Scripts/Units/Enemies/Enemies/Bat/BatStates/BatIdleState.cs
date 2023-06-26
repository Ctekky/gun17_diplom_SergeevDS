using UnityEngine;

namespace Metroidvania.Enemy
{
    public class BatIdleState : EnemyIdleState
    {
        private readonly BatEnemy _batEnemy;
        private bool _isPlayerInMaxAggroRange;
        public BatIdleState(BaseEnemy enemy, EnemyStateMachine stateMachine, EnemyData enemyData, string animBoolName,
            BatEnemy batEnemy) :
            base(enemy, stateMachine, enemyData, animBoolName)
        {
            _batEnemy = batEnemy;
        }
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (!_isPlayerInMaxAggroRange) return;
            StateMachine.ChangeState(_batEnemy.ChasingPlayerState);
        }

        protected override void DoChecks()
        {
            base.DoChecks();
            _isPlayerInMaxAggroRange = _batEnemy.CheckPlayerInMaxRange();

        }
    }
}