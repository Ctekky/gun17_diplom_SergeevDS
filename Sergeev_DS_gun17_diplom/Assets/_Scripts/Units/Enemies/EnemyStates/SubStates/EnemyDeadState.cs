using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Metroidvania.BaseUnit;

namespace Metroidvania.Enemy
{
    public class EnemyDeadState : EnemyState
    {
        private Movement Movement => _movement ? _movement : Unit.GetUnitComponent<Movement>(ref _movement);

        private CollisionChecks CollisionChecks => _collisionChecks ? CollisionChecks : Unit.GetUnitComponent<CollisionChecks>(ref _collisionChecks);
        protected Death Death => _death ? _death : Unit.GetUnitComponent<Death>(ref _death);
        private Movement _movement;
        private CollisionChecks _collisionChecks;
        private Death _death;

        protected EnemyDeadState(BaseEnemy enemy, EnemyStateMachine stateMachine, EnemyData enemyData, string animBoolName) : base(enemy, stateMachine, enemyData, animBoolName)
        {
        }
        public override void Enter()
        {
            base.Enter();
            Movement?.SetVelocityZero();

        }
        public override void Exit()
        {
            base.Exit();
            //Enemy.gameObject.SetActive(false);
        }
    }
}


