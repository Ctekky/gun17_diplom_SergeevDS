using Metroidvania.BaseUnit;

namespace Metroidvania.Enemy
{
    public class EnemyGroundedState : EnemyState
    {
        protected Movement Movement => _movement ? _movement : Unit.GetUnitComponent<Movement>(ref _movement);

        protected CollisionChecks CollisionChecks => _collisionChecks ? _collisionChecks : Unit.GetUnitComponent<CollisionChecks>(ref _collisionChecks);
        private Movement _movement;
        private CollisionChecks _collisionChecks;
        protected bool IsPlayerInMinAggroRange;
        public EnemyGroundedState(BaseEnemy enemy, EnemyStateMachine stateMachine, EnemyData enemyData, string animBoolName) : base(enemy, stateMachine, enemyData, animBoolName)
        {
        }
        public override void DoChecks()
        {
            base.DoChecks();
            IsPlayerInMinAggroRange = Enemy.CheckPlayerInMinRange();
        }
    }

}

