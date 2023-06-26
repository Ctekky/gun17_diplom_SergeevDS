namespace Metroidvania.Enemy
{
    public class BatMoveState : EnemyMoveState
    {
        public BatMoveState(BaseEnemy enemy, EnemyStateMachine stateMachine, EnemyData enemyData, string animBoolName) :
            base(enemy, stateMachine, enemyData, animBoolName)
        {
        }
    }
}