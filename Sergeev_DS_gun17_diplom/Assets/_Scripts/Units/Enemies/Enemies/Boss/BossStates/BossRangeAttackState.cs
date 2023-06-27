using UnityEngine;

namespace Metroidvania.Enemy
{
    public class BossRangeAttackState : EnemyRangeAttackState
    {
        private readonly BossEnemy _bossEnemy;

        public BossRangeAttackState(BaseEnemy enemy, EnemyStateMachine stateMachine, EnemyData enemyData,
            string animBoolName, Transform attackPosition, BossEnemy bossEnemy) : base(enemy, stateMachine, enemyData,
            animBoolName,
            attackPosition)
        {
            _bossEnemy = bossEnemy;
        }

        public override void Enter()
        {
            base.Enter();
            Movement?.FlipToTarget(_bossEnemy.transform.position, _bossEnemy.GetPlayerPosition().position);
        }

        public override void AnimationTrigger()
        {
            base.AnimationTrigger();
            _bossEnemy.RangeAttack();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (!IsAnimationEnd) return;
            StateMachine.ChangeState(_bossEnemy.IdleState);
        }
    }
}