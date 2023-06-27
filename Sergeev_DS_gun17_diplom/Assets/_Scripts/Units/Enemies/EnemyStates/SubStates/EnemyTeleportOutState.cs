using Metroidvania.BaseUnit;
using UnityEngine;

namespace Metroidvania.Enemy
{
    public class EnemyTeleportOutState : EnemyState
    {
        protected Movement Movement => _movement ? _movement : Unit.GetUnitComponent<Movement>(ref _movement);

        private CollisionChecks CollisionChecks => _collisionChecks
            ? _collisionChecks
            : Unit.GetUnitComponent<CollisionChecks>(ref _collisionChecks);

        private Movement _movement;
        private CollisionChecks _collisionChecks;
        protected bool IsPlayerInMaxAggroRange;

        protected EnemyTeleportOutState(BaseEnemy enemy, EnemyStateMachine stateMachine, EnemyData enemyData,
            string animBoolName) : base(enemy, stateMachine, enemyData, animBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();
            Enemy.AnimToStateMachine.TeleportOutState = this;
            IsAnimationEnd = false;
            Movement?.SetVelocityZero();
        }

        public override void AnimationEndTrigger()
        {
            base.AnimationEndTrigger();
            IsAnimationEnd = true;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            Movement?.SetVelocityZero();
        }

        protected override void DoChecks()
        {
            base.DoChecks();
            IsPlayerInMaxAggroRange = Enemy.CheckPlayerInMaxRange();
        }
    }
}