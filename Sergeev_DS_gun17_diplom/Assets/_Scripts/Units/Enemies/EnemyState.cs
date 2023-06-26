using Metroidvania.BaseUnit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Metroidvania.Enemy
{
    public class EnemyState
    {
        protected readonly BaseEnemy Enemy;
        protected readonly EnemyStateMachine StateMachine;
        protected readonly EnemyData EnemyData;
        protected readonly Unit Unit;

        protected bool IsExitingState;
        protected bool IsAnimationEnd;
        protected float StartTime;
        private readonly string _animBoolName;

        protected EnemyState(BaseEnemy enemy, EnemyStateMachine stateMachine, EnemyData enemyData, string animBoolName)
        {
            Enemy = enemy;
            StateMachine = stateMachine;
            EnemyData = enemyData;
            _animBoolName = animBoolName;
            Unit = enemy.Unit;
        }

        public virtual void Enter()
        {
            DoChecks();
            StartTime = Time.time;
            Enemy.Animator.SetBool(_animBoolName, true);
            IsAnimationEnd = false;
            IsExitingState = false;
        }

        public virtual void Exit()
        {
            Enemy.Animator.SetBool(_animBoolName, false);
            IsExitingState = true;
        }

        public virtual void LogicUpdate()
        {
        }

        public virtual void PhysicsUpdate()
        {
            DoChecks();
        }

        protected virtual void DoChecks()
        {
        }

        public virtual void AnimationTrigger()
        {
        }

        public virtual void AnimationEndTrigger() => IsAnimationEnd = true;
    }
}