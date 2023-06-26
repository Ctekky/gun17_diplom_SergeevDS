namespace Metroidvania.Enemy
{
    public class BoarLookForPlayerState : EnemyLookForPlayerState
    {
        private readonly BoarEnemy _boarEnemy;

        public BoarLookForPlayerState(BaseEnemy enemy, EnemyStateMachine stateMachine, EnemyData enemyData,
            string animBoolName, BoarEnemy boarEnemy) : base(enemy, stateMachine, enemyData, animBoolName)
        {
            _boarEnemy = boarEnemy;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (IsPlayerInMinAggroRange)
            {
                StateMachine.ChangeState(_boarEnemy.DetectedPlayerState);
            }
            else if (IsAllTurnsDone)
            {
                StateMachine.ChangeState(_boarEnemy.MoveState);
            }
        }
    }
}