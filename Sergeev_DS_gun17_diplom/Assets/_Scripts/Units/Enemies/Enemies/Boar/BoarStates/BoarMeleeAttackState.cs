using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Metroidvania.Enemy
{
    public class BoarMeleeAttackState : EnemyMeleeAttackState
    {
        BoarEnemy boarEnemy;
        public BoarMeleeAttackState(BaseEnemy enemy, EnemyStateMachine stateMachine, EnemyData enemyData, string animBoolName, Transform attackPosition, BoarEnemy boarEnemy) : base(enemy, stateMachine, enemyData, animBoolName, attackPosition)
        {
            this.boarEnemy = boarEnemy;
        }
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if(isAnimationEnd)
            {
                if (isPlayerInMinAggroRange) stateMachine.ChangeState(boarEnemy.DetectedPlayerState);
                else stateMachine.ChangeState(boarEnemy.LookForPlayerState);

            }
        }
    }
}

