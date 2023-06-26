namespace Metroidvania.Enemy
{
    public class BatDeadState : EnemyDeadState
    {
        protected BatDeadState(BaseEnemy enemy, EnemyStateMachine stateMachine, EnemyData enemyData,
            string animBoolName) : base(enemy, stateMachine, enemyData, animBoolName)
        {
        }
    }
}