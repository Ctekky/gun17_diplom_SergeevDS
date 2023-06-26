namespace Metroidvania.Enemy
{
    public class SkeletonLookForPlayerState : EnemyLookForPlayerState
    {
        private readonly SkeletonEnemy _skeletonEnemy;
        private bool _isPlayerInMaxAggroRange;

        public SkeletonLookForPlayerState(BaseEnemy enemy, EnemyStateMachine stateMachine, EnemyData enemyData,
            string animBoolName, SkeletonEnemy skeletonEnemy) : base(enemy, stateMachine, enemyData, animBoolName)
        {
            _skeletonEnemy = skeletonEnemy;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (_isPlayerInMaxAggroRange)
            {
                StateMachine.ChangeState(_skeletonEnemy.ChasingPlayerState);
            }
            else if (IsAllTurnsDone)
            {
                StateMachine.ChangeState(_skeletonEnemy.MoveState);
            }
        }

        protected override void DoChecks()
        {
            base.DoChecks();
            _isPlayerInMaxAggroRange = _skeletonEnemy.CheckPlayerInMaxRange();
        }
    }
}