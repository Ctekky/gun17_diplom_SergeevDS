using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Metroidvania.Enemy
{
    public class BossIdleState : EnemyIdleState
    {
        private readonly BossEnemy _bossEnemy;
        private bool _isPlayerInMaxAggroRange;

        public BossIdleState(BaseEnemy enemy, EnemyStateMachine stateMachine, EnemyData enemyData,
            string animBoolName, BossEnemy bossEnemy) : base(enemy, stateMachine, enemyData, animBoolName)
        {
            _bossEnemy = bossEnemy;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (!_isPlayerInMaxAggroRange) return;
            if (Time.time <= StartTime + IdleTime) return;
            var teleportVariant = TeleportVariant.Above;
            if (_bossEnemy.beforeLastTeleportVariant != TeleportVariant.Behind &&
                _bossEnemy.lastTeleportVariant != TeleportVariant.Behind)
            {
                teleportVariant = TeleportVariant.Behind;
            }
            else
            {
                teleportVariant = (TeleportVariant)Random.Range(0, Enum.GetValues(typeof(TeleportVariant)).Length);
            }

            _bossEnemy.beforeLastTeleportVariant = _bossEnemy.lastTeleportVariant;
            switch (teleportVariant)
            {
                case TeleportVariant.Above:
                    StateMachine.ChangeState(_bossEnemy.TeleportInAboveState);
                    break;
                case TeleportVariant.Left:
                    StateMachine.ChangeState(_bossEnemy.TeleportInLeftState);
                    break;
                case TeleportVariant.Right:
                    StateMachine.ChangeState(_bossEnemy.TeleportInRightState);
                    break;
                case TeleportVariant.Behind:
                    StateMachine.ChangeState(_bossEnemy.TeleportInBehindState);
                    break;
                default:
                    break;
            }
        }

        protected override void DoChecks()
        {
            base.DoChecks();
            _isPlayerInMaxAggroRange = _bossEnemy.CheckPlayerInMaxRange();
        }
    }
}