using UnityEngine;

namespace Metroidvania.Enemy
{
    public class SkeletonArcherIdleState : EnemyIdleState
    {
        private readonly SkeletonArcherEnemy _skeletonArcherEnemy;
        private bool _isPlayerInMaxAggroRange;

        public SkeletonArcherIdleState(BaseEnemy enemy, EnemyStateMachine stateMachine, EnemyData enemyData,
            string animBoolName, SkeletonArcherEnemy skeletonArcherEnemy) : base(enemy, stateMachine, enemyData,
            animBoolName)
        {
            _skeletonArcherEnemy = skeletonArcherEnemy;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (_isPlayerInMaxAggroRange)
            {
                StateMachine.ChangeState(_skeletonArcherEnemy.AttackState);
            }
            else if (Time.time >= StartTime + IdleTime)
                StateMachine.ChangeState(_skeletonArcherEnemy.LookForPlayerState);
        }

        protected override void DoChecks()
        {
            base.DoChecks();
            _isPlayerInMaxAggroRange = _skeletonArcherEnemy.CheckPlayerInMaxRange();
        }
    }
}