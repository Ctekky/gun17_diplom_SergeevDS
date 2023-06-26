using UnityEngine;

namespace Metroidvania.Enemy
{
    public class SkeletonMeleeAttackState : EnemyMeleeAttackState
    {
        private readonly SkeletonEnemy _skeletonEnemy;

        public SkeletonMeleeAttackState(BaseEnemy enemy, EnemyStateMachine stateMachine, EnemyData enemyData,
            string animBoolName, Transform attackPosition, SkeletonEnemy skeletonEnemy) : base(enemy, stateMachine,
            enemyData, animBoolName,
            attackPosition)
        {
            _skeletonEnemy = skeletonEnemy;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (!IsAnimationEnd) return;
            StateMachine.ChangeState(IsPlayerInMinAggroRange
                ? _skeletonEnemy.ChasingPlayerState
                : _skeletonEnemy.LookForPlayerState);
        }
    }
}