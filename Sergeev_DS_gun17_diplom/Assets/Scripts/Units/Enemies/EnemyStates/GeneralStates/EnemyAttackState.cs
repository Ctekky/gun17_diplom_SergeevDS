using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Metroidvania.Enemy
{
    public class EnemyAttackState : EnemyState
    {
        protected Transform attackPosition;
        protected bool isPlayerInMinAggroRange;
        public EnemyAttackState(BaseEnemy enemy, EnemyStateMachine stateMachine, EnemyData enemyData, string animBoolName, Transform attackPosition) : base(enemy, stateMachine, enemyData, animBoolName)
        {
            this.attackPosition = attackPosition;
        }
        public override void Enter()
        {
            base.Enter();
            enemy.animToStateMachine.attackState = this;
            isAnimationEnd = false;
            enemy.SetVelocityZero();
        }

        public override void AnimationEndTrigger()
        {
            base.AnimationEndTrigger();
            isAnimationEnd = true;
        }
        public override void DoChecks()
        {
            base.DoChecks();
            isPlayerInMinAggroRange = enemy.CheckPlayerInMinRange();
        }

    }
}

