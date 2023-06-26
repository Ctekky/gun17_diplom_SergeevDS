using UnityEngine;

namespace Metroidvania.Enemy
{
    public class BatChasingPlayerState : EnemyChasingPlayerState
    {
        private readonly BatEnemy _batEnemy;
        private bool _isPlayerInMaxAggroRange;

        public BatChasingPlayerState(BaseEnemy enemy, EnemyStateMachine stateMachine, EnemyData enemyData,
            string animBoolName, BatEnemy batEnemy) : base(enemy, stateMachine, enemyData, animBoolName)
        {
            _batEnemy = batEnemy;
        }

        public override void LogicUpdate()
        {
            base.PhysicsUpdate();
            if (PerformCloseRangeAction)
            {
                StateMachine.ChangeState(_batEnemy.MeleeAttackState);
            }
            else if (_isPlayerInMaxAggroRange)
            {
                Movement?.SetVelocityToTarget(_batEnemy.GetPlayerPosition(),
                    EnemyData.chargeVelocity);
            }
            else
            {
                StateMachine.ChangeState(_batEnemy.ReturnToStartingPositionState);
            }
        }

        public override void DoChecks()
        {
            base.DoChecks();
            _isPlayerInMaxAggroRange = _batEnemy.CheckPlayerInMaxRange();
        }
    }
}