using UnityEngine;

namespace Metroidvania.Enemy
{
    public class BoarMeleeAttackState : EnemyMeleeAttackState
    {
        private readonly BoarEnemy _boarEnemy;

        public BoarMeleeAttackState(BaseEnemy enemy, EnemyStateMachine stateMachine, EnemyData enemyData,
            string animBoolName, Transform attackPosition, BoarEnemy boarEnemy) : base(enemy, stateMachine, enemyData,
            animBoolName, attackPosition)
        {
            _boarEnemy = boarEnemy;
        }
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (!IsAnimationEnd) return;
            StateMachine.ChangeState(IsPlayerInMinAggroRange
                ? _boarEnemy.DetectedPlayerState
                : _boarEnemy.LookForPlayerState);
            //if (IsPlayerInMinAggroRange) StateMachine.ChangeState(_boarEnemy.DetectedPlayerState);
            //else StateMachine.ChangeState(_boarEnemy.LookForPlayerState);
        }
    }
}