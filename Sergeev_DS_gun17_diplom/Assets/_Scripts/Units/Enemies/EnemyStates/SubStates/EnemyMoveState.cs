namespace Metroidvania.Enemy
{
    public class EnemyMoveState : EnemyGroundedState
    {
        protected bool IsDetectingWall;
        protected bool IsDetectingLedge;

        protected EnemyMoveState(BaseEnemy enemy, EnemyStateMachine stateMachine, EnemyData enemyData,
            string animBoolName) : base(enemy, stateMachine, enemyData, animBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();
            Movement?.SetVelocityX(EnemyData.movementVelocity * Movement.FacingDirection);
        }

        protected override void DoChecks()
        {
            base.DoChecks();
            if (CollisionChecks)
            {
                IsDetectingLedge = CollisionChecks.LedgeVertical;
                IsDetectingWall = CollisionChecks.WallFront;
            }

            IsPlayerInMinAggroRange = Enemy.CheckPlayerInMinRange();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (IsExitingState) return;
            Movement?.SetVelocityX(EnemyData.movementVelocity * Movement.FacingDirection);
        }
    }
}