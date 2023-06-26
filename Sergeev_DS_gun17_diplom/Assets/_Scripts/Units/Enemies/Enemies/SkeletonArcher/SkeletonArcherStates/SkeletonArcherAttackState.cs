using UnityEngine;

namespace Metroidvania.Enemy
{
    public class SkeletonArcherAttackState : EnemyRangeAttackState
    {
        private readonly SkeletonArcherEnemy _skeletonArcherEnemy;
        private bool _isPlayerInMaxAggroRange;
        public SkeletonArcherAttackState(BaseEnemy enemy, EnemyStateMachine stateMachine, EnemyData enemyData,
            string animBoolName, Transform attackPosition, SkeletonArcherEnemy skeletonArcherEnemy) : base(enemy,
            stateMachine, enemyData,
            animBoolName, attackPosition)
        {
            _skeletonArcherEnemy = skeletonArcherEnemy;
        }

        public override void Enter()
        {
            base.Enter();
            Movement?.FlipToTarget(_skeletonArcherEnemy.transform.position, _skeletonArcherEnemy.GetPlayerPosition().position);
        }

        public override void AnimationTrigger()
        {
            base.AnimationTrigger();
            _skeletonArcherEnemy.RangeAttack();
        }
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (!IsAnimationEnd) return;
            StateMachine.ChangeState(_isPlayerInMaxAggroRange
                ? _skeletonArcherEnemy.AttackState
                : _skeletonArcherEnemy.LookForPlayerState);
        }
        
        protected override void DoChecks()
        {
            base.DoChecks();
            _isPlayerInMaxAggroRange = _skeletonArcherEnemy.CheckPlayerInMaxRange();
        }
    }
}