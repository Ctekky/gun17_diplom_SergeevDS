using UnityEngine;

namespace Metroidvania.Enemy
{
    public class SkeletonMoveState : EnemyMoveState
    {
        private readonly SkeletonEnemy _skeletonEnemy;
        private float _movementTimer;
        private bool _isPlayerInMaxAggroRange;

        public SkeletonMoveState(BaseEnemy enemy, EnemyStateMachine stateMachine, EnemyData enemyData,
            string animBoolName, SkeletonEnemy skeletonEnemy) : base(enemy, stateMachine, enemyData, animBoolName)
        {
            _skeletonEnemy = skeletonEnemy;
        }

        public override void Enter()
        {
            base.Enter();
            _movementTimer = Random.Range(EnemyData.minMovementTimer, EnemyData.maxMovementTimer);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (_isPlayerInMaxAggroRange)
            {
                StateMachine.ChangeState(_skeletonEnemy.ChasingPlayerState);
            }
            else if (IsDetectingWall || !IsDetectingLedge || Time.time >= StartTime + _movementTimer)
            {
                _skeletonEnemy.IdleState.SetFlipAfterIdle(true);
                StateMachine.ChangeState(_skeletonEnemy.IdleState);
            }
        }

        protected override void DoChecks()
        {
            base.DoChecks();
            _isPlayerInMaxAggroRange = _skeletonEnemy.CheckPlayerInMaxRange();
        }
    }
}