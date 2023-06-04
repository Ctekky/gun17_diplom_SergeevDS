using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Metroidvania.Enemy
{
    public class BoarMeleeAttackState : EnemyMeleeAttackState
    {
        readonly BoarEnemy _boarEnemy;
        public BoarMeleeAttackState(BaseEnemy enemy, EnemyStateMachine stateMachine, EnemyData enemyData, string animBoolName, Transform attackPosition, BoarEnemy boarEnemy) : base(enemy, stateMachine, enemyData, animBoolName, attackPosition)
        {
            _boarEnemy = boarEnemy;
        }
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if(IsAnimationEnd)
            {
                if (IsPlayerInMinAggroRange) StateMachine.ChangeState(_boarEnemy.DetectedPlayerState);
                else StateMachine.ChangeState(_boarEnemy.LookForPlayerState);

            }
        }
    }
}

