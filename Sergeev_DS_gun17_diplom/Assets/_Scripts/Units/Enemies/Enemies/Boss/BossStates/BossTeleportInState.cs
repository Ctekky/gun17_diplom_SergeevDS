using System;
using UnityEngine;

namespace Metroidvania.Enemy
{
    public class BossTeleportInState : EnemyTeleportInState
    {
        private readonly BossEnemy _bossEnemy;
        private readonly TeleportVariant _teleportVariant;

        public BossTeleportInState(BaseEnemy enemy, EnemyStateMachine stateMachine, EnemyData enemyData,
            string animBoolName, BossEnemy bossEnemy, TeleportVariant tp) : base(enemy, stateMachine, enemyData,
            animBoolName)
        {
            _bossEnemy = bossEnemy;
            _teleportVariant = tp;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if(!IsAnimationEnd) return;
            switch (_teleportVariant)
            {
                case TeleportVariant.Above:
                    _bossEnemy.transform.position =
                        new Vector2(_bossEnemy.GetPlayerPosition().position.x + EnemyData.teleportOffsetAbove.x,
                            _bossEnemy.GetPlayerPosition().position.y + EnemyData.teleportOffsetAbove.y);
                    break;
                case TeleportVariant.Left:
                    _bossEnemy.transform.position =
                        new Vector2(_bossEnemy.GetPlayerPosition().position.x + EnemyData.teleportOffsetLeft.x,
                            _bossEnemy.GetPlayerPosition().position.y + EnemyData.teleportOffsetLeft.y);
                    break;
                case TeleportVariant.Right:
                    _bossEnemy.transform.position =
                        new Vector2(_bossEnemy.GetPlayerPosition().position.x + EnemyData.teleportOffsetRight.x,
                            _bossEnemy.GetPlayerPosition().position.y + EnemyData.teleportOffsetRight.y);
                    break;
                case TeleportVariant.Behind:
                    _bossEnemy.transform.position =
                        new Vector2(
                            _bossEnemy.GetPlayerPosition().position.x +
                            (EnemyData.teleportOffsetBehind.x * Movement.FacingDirection),
                            _bossEnemy.GetPlayerPosition().position.y + EnemyData.teleportOffsetBehind.y);
                    break;
                default:
                    break;
            }
            _bossEnemy.lastTeleportVariant = _teleportVariant;
            Movement?.FlipToTarget(_bossEnemy.transform.position, _bossEnemy.GetPlayerPosition().position);
            StateMachine.ChangeState(_bossEnemy.TeleportOutState);
        }
    }
}