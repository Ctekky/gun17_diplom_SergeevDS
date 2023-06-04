using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Metroidvania.Enemy
{
    public class EnemyIdleState : EnemyGroundedState
    {
        protected bool FlipAfterIdle;
        protected float IdleTime;
        protected bool IsIdleTimeOver;
        public EnemyIdleState(BaseEnemy enemy, EnemyStateMachine stateMachine, EnemyData enemyData, string animBoolName) : base(enemy, stateMachine, enemyData, animBoolName)
        {
        }
        public override void Enter()
        {
            base.Enter();
            Movement?.SetVelocityZero();
            IsIdleTimeOver = false;
            SetRandomIdleTime();
        }
        public override void Exit()
        {
            base.Exit();
            IsIdleTimeOver = true;
            if (FlipAfterIdle) Movement?.Flip();
        }
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (IsExitingState) return;
            Movement?.SetVelocityZero();
        }
        public void SetFlipAfterIdle(bool flip) => FlipAfterIdle = flip;
        private void SetRandomIdleTime() => IdleTime = Random.Range(EnemyData.minIdleTime, EnemyData.maxIdleTime);
    }
}

