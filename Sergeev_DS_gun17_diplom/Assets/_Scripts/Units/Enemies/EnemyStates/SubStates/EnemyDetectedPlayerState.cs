using Metroidvania.BaseUnit;
namespace Metroidvania.Enemy
{
    public class EnemyDetectedPlayerState : EnemyState
    {
        protected Movement Movement => _movement ? _movement : Unit.GetUnitComponent<Movement>(ref _movement);
        private CollisionChecks CollisionChecks => _collisionChecks ? _collisionChecks : Unit.GetUnitComponent<CollisionChecks>(ref _collisionChecks);
        private Movement _movement;
        private CollisionChecks _collisionChecks;
        protected bool IsPlayerInMinAggroRange;
        protected bool IsPlayerImMaxAggroRange;
        protected bool PerformLongRangeAction;
        protected bool PerformCloseRangeAction;

        protected EnemyDetectedPlayerState(BaseEnemy enemy, EnemyStateMachine stateMachine, EnemyData enemyData, string animBoolName) : base(enemy, stateMachine, enemyData, animBoolName)
        {
        }
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (IsExitingState) return;
        }

        protected override void DoChecks()
        {
            base.DoChecks();
            IsPlayerInMinAggroRange = Enemy.CheckPlayerInMinRange();
            IsPlayerImMaxAggroRange = Enemy.CheckPlayerInMaxRange();
            PerformCloseRangeAction = Enemy.CheckPlayerInCloseRangeAction();
            PerformLongRangeAction = Enemy.CheckPlayerInLongRangeAction();
        }
    }
}

