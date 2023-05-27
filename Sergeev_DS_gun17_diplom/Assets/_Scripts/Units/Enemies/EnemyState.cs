using Metroidvania.BaseUnit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Metroidvania.Enemy
{
    public class EnemyState
    {
        protected BaseEnemy enemy;
        protected EnemyStateMachine stateMachine;
        protected EnemyData enemyData;
        protected Unit unit;

        protected bool isExitingState;
        protected bool isAnimationEnd;
        protected float startTime;
        private string animBoolName;

        public EnemyState(BaseEnemy enemy, EnemyStateMachine stateMachine, EnemyData enemyData, string animBoolName)
        {
            this.enemy = enemy;
            this.stateMachine = stateMachine;
            this.enemyData = enemyData;
            this.animBoolName = animBoolName;
            unit = enemy.Unit;
        }

        public virtual void Enter()
        {
            DoChecks(); 
            startTime = Time.time;
            enemy.Animator.SetBool(animBoolName, true);
            isAnimationEnd = false;
            isExitingState = false;
        }
        public virtual void Exit()
        {
            enemy.Animator.SetBool(animBoolName, false);
            isExitingState = true;
        }
        public virtual void LogicUpdate() { }
        public virtual void PhysicsUpdate()
        {
            DoChecks();
        }
        public virtual void DoChecks() { }
        public virtual void AnimationTrigger() { }
        public virtual void AnimationEndTrigger() => isAnimationEnd = true;
    }
}

