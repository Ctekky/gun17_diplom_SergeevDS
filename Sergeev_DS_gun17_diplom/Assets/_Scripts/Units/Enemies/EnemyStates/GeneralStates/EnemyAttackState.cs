using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Metroidvania.BaseUnit;
namespace Metroidvania.Enemy
{
    public class EnemyAttackState : EnemyState
    {
        protected Movement Movement
        {
            get => movement ?? unit.GetUnitComponent<Movement>(ref movement);
        }
        private CollisionChecks CollisionChecks
        {
            get => collisionChecks ?? unit.GetUnitComponent<CollisionChecks>(ref collisionChecks);
        }
        private Movement movement;
        private CollisionChecks collisionChecks;
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
            Movement?.SetVelocityZero();
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
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            Movement?.SetVelocityZero();
        }

    }
}

