using UnityEngine;

namespace Metroidvania.Enemy
{
    public class BossTeleportOutState : EnemyTeleportOutState
    {
        private readonly BossEnemy _bossEnemy;

        public BossTeleportOutState(BaseEnemy enemy, EnemyStateMachine stateMachine, EnemyData enemyData,
            string animBoolName, BossEnemy bossEnemy) : base(enemy, stateMachine, enemyData, animBoolName)
        {
            _bossEnemy = bossEnemy;
        }

        public override void Enter()
        {
            base.Enter();
            Movement?.FlipToTarget(_bossEnemy.transform.position, _bossEnemy.GetPlayerPosition().position);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (!IsAnimationEnd) return;
            if (_bossEnemy.lastTeleportVariant == TeleportVariant.Behind)
            {
                StateMachine.ChangeState(_bossEnemy.MeleeAttackState);
            }
            else
            {
                if (Random.Range(0, 100) >= 35)
                {
                    StateMachine.ChangeState(_bossEnemy.RangeAttackState);
                }
                else
                {
                    StateMachine.ChangeState(_bossEnemy.IdleState);
                    //TODO spawn bats
                }
            }
        }
    }
}