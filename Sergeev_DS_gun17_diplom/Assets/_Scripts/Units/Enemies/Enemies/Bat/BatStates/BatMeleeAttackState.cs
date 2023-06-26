using UnityEngine;

namespace Metroidvania.Enemy
{
    public class BatMeleeAttackState : EnemyMeleeAttackState
    {
        private readonly BatEnemy _batEnemy;
        private bool _isPlayerInMaxAggroRange;
        public BatMeleeAttackState(BaseEnemy enemy, EnemyStateMachine stateMachine, EnemyData enemyData,
            string animBoolName, Transform attackPosition, BatEnemy batEnemy) : base(enemy, stateMachine, enemyData,
            animBoolName,
            attackPosition)
        {
            _batEnemy = batEnemy;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (!IsAnimationEnd) return;
            StateMachine.ChangeState(_isPlayerInMaxAggroRange
                ? _batEnemy.ChasingPlayerState
                : _batEnemy.ReturnToStartingPositionState);
        }

        protected override void DoChecks()
        {
            base.DoChecks();
            _isPlayerInMaxAggroRange = _batEnemy.CheckPlayerInMaxRange();
        }
    }
}