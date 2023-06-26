using UnityEngine;

namespace Metroidvania.Enemy
{
    public class EnemyIdleState : EnemyGroundedState
    {
        private bool _flipAfterIdle;
        protected float IdleTime;
        protected bool IsIdleTimeOver;

        protected EnemyIdleState(BaseEnemy enemy, EnemyStateMachine stateMachine, EnemyData enemyData,
            string animBoolName) : base(enemy, stateMachine, enemyData, animBoolName)
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
            if (_flipAfterIdle) Movement?.Flip();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (IsExitingState) return;
            Movement?.SetVelocityZero();
        }

        public void SetFlipAfterIdle(bool flip) => _flipAfterIdle = flip;
        private void SetRandomIdleTime() => IdleTime = Random.Range(EnemyData.minIdleTime, EnemyData.maxIdleTime);
    }
}