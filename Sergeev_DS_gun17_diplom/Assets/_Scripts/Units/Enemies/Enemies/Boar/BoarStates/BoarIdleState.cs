using UnityEngine;

namespace Metroidvania.Enemy
{
    public class BoarIdleState : EnemyIdleState
    {
        private readonly BoarEnemy _boarEnemy;
        public BoarIdleState(BaseEnemy enemy, EnemyStateMachine stateMachine, EnemyData enemyData, string animBoolName, BoarEnemy boarEnemy) : base(enemy, stateMachine, enemyData, animBoolName)
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
            else if (Time.time >= StartTime + IdleTime) StateMachine.ChangeState(_boarEnemy.MoveState);
        }
    }
}

