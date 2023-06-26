using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Metroidvania.Enemy
{
    public class SkeletonChasingPlayerState : EnemyChasingPlayerState
    {
        private readonly SkeletonEnemy _skeletonEnemy;
        private bool _isPlayerInMaxAggroRange;

        public SkeletonChasingPlayerState(BaseEnemy enemy, EnemyStateMachine stateMachine, EnemyData enemyData,
            string animBoolName, SkeletonEnemy skeletonEnemy) : base(enemy, stateMachine, enemyData, animBoolName)
        {
            _skeletonEnemy = skeletonEnemy;
        }

        public override void LogicUpdate()
        {
            base.PhysicsUpdate();
            if (PerformCloseRangeAction)
            {
                StateMachine.ChangeState(Random.Range(0, 100) >= 50
                    ? _skeletonEnemy.MeleeAttackState1
                    : _skeletonEnemy.MeleeAttackState2);
            }
            else if (_isPlayerInMaxAggroRange)
            {
                Movement?.SetVelocityToTarget(_skeletonEnemy.GetPlayerPosition(),
                    EnemyData.chargeVelocity);
            }
            else
            {
                StateMachine.ChangeState(_skeletonEnemy.LookForPlayerState);
            }
        }

        protected override void DoChecks()
        {
            base.DoChecks();
            _isPlayerInMaxAggroRange = _skeletonEnemy.CheckPlayerInMaxRange();
        }
    }
}