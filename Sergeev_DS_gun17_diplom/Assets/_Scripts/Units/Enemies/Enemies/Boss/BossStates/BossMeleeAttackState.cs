using UnityEngine;

namespace Metroidvania.Enemy
{
    public class BossMeleeAttackState : EnemyMeleeAttackState
    {
        private readonly BossEnemy _bossEnemy;

        public BossMeleeAttackState(BaseEnemy enemy, EnemyStateMachine stateMachine, EnemyData enemyData,
            string animBoolName, Transform attackPosition, BossEnemy bossEnemy) : base(enemy, stateMachine, enemyData,
            animBoolName,
            attackPosition)
        {
            _bossEnemy = bossEnemy;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (!IsAnimationEnd) return;
            StateMachine.ChangeState(_bossEnemy.IdleState);
        }
    }
}