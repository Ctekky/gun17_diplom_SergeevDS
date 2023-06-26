using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Metroidvania.BaseUnit;

namespace Metroidvania.Enemy
{
    public class EnemyAttackState : EnemyState
    {
        protected Movement Movement => _movement ? _movement : Unit.GetUnitComponent<Movement>(ref _movement);
        private Movement _movement;
        protected UnitStats UnitStats => _unitStats ? _unitStats : Unit.GetUnitComponent<UnitStats>(ref _unitStats);
        private UnitStats _unitStats;
        protected readonly Transform AttackPosition;
        protected bool IsPlayerInMinAggroRange;

        protected EnemyAttackState(BaseEnemy enemy, EnemyStateMachine stateMachine, EnemyData enemyData,
            string animBoolName, Transform attackPosition) : base(enemy, stateMachine, enemyData, animBoolName)
        {
            AttackPosition = attackPosition;
        }

        public override void Enter()
        {
            base.Enter();
            Enemy.AnimToStateMachine.AttackState = this;
            IsAnimationEnd = false;
            Movement?.SetVelocityZero();
        }

        public override void AnimationEndTrigger()
        {
            base.AnimationEndTrigger();
            IsAnimationEnd = true;
        }

        public override void DoChecks()
        {
            base.DoChecks();
            IsPlayerInMinAggroRange = Enemy.CheckPlayerInMinRange();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            Movement?.SetVelocityZero();
        }
    }
}