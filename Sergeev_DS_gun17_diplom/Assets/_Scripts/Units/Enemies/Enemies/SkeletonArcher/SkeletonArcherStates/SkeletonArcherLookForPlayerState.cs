namespace Metroidvania.Enemy
{
    public class SkeletonArcherLookForPlayerState : EnemyLookForPlayerState
    {
        private readonly SkeletonArcherEnemy _skeletonArcherEnemy;
        private bool _isPlayerInMaxAggroRange;

        public SkeletonArcherLookForPlayerState(BaseEnemy enemy, EnemyStateMachine stateMachine, EnemyData enemyData,
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
            else if (IsAllTurnsDone)
            {
                StateMachine.ChangeState(_skeletonArcherEnemy.IdleState);
            }
        }

        protected override void DoChecks()
        {
            base.DoChecks();
            _isPlayerInMaxAggroRange = _skeletonArcherEnemy.CheckPlayerInMaxRange();
        }
    }
}