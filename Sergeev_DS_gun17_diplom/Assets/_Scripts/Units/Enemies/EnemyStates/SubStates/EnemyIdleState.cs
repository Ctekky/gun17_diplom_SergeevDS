using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Metroidvania.Enemy
{
    public class EnemyIdleState : EnemyGroundedState
    {
        protected bool flipAfterIdle;
        protected float idleTime;
        protected bool isIdleTimeOver;
        public EnemyIdleState(BaseEnemy enemy, EnemyStateMachine stateMachine, EnemyData enemyData, string animBoolName) : base(enemy, stateMachine, enemyData, animBoolName)
        {
        }
        public override void Enter()
        {
            base.Enter();
            Movement?.SetVelocityZero();
            isIdleTimeOver = false;
            SetRandomIdleTime();
        }
        public override void Exit()
        {
            base.Exit();
            isIdleTimeOver = true;
            if (flipAfterIdle) Movement?.Flip();
        }
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (isExitingState) return;
            Movement?.SetVelocityZero();
        }
        public void SetFlipAfterIdle(bool flip) => flipAfterIdle = flip;
        private void SetRandomIdleTime() => idleTime = Random.Range(enemyData.minIdleTime, enemyData.maxIdleTime);
    }
}

