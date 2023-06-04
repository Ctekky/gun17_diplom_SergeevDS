using Metroidvania.BaseUnit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Metroidvania.Enemy
{
    public class EnemyState
    {
        protected BaseEnemy Enemy;
        protected EnemyStateMachine StateMachine;
        protected EnemyData EnemyData;
        protected Unit Unit;

        protected bool IsExitingState;
        protected bool IsAnimationEnd;
        protected float StartTime;
        private string _animBoolName;

        public EnemyState(BaseEnemy enemy, EnemyStateMachine stateMachine, EnemyData enemyData, string animBoolName)
        {
            this.Enemy = enemy;
            this.StateMachine = stateMachine;
            this.EnemyData = enemyData;
            this._animBoolName = animBoolName;
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
        public virtual void LogicUpdate() { }
        public virtual void PhysicsUpdate()
        {
            DoChecks();
        }
        public virtual void DoChecks() { }
        public virtual void AnimationTrigger() { }
        public virtual void AnimationEndTrigger() => IsAnimationEnd = true;
    }
}

