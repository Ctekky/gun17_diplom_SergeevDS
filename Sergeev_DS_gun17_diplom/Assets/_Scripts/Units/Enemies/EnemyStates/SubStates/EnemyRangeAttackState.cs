using Metroidvania.Combat.Projectile;
using UnityEngine;

namespace Metroidvania.Enemy
{
    public class EnemyRangeAttackState : EnemyAttackState
    {
        protected EnemyRangeAttackState(BaseEnemy enemy, EnemyStateMachine stateMachine, EnemyData enemyData,
            string animBoolName, Transform attackPosition) : base(enemy, stateMachine, enemyData, animBoolName,
            attackPosition)
        {
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (IsExitingState) return;
        }
    }
}