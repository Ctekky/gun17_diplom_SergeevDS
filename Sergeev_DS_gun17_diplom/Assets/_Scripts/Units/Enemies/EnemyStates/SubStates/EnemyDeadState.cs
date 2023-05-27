using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Metroidvania.BaseUnit;

namespace Metroidvania.Enemy
{
    public class EnemyDeadState : EnemyState
    {
        protected Movement Movement => movement ? movement : unit.GetUnitComponent<Movement>(ref movement);

        private CollisionChecks CollisionChecks => collisionChecks ? CollisionChecks : unit.GetUnitComponent<CollisionChecks>(ref collisionChecks);
        protected Death Death => death ? death : unit.GetUnitComponent<Death>(ref death);
        private Movement movement;
        private CollisionChecks collisionChecks;
        private Death death;

        public EnemyDeadState(BaseEnemy enemy, EnemyStateMachine stateMachine, EnemyData enemyData, string animBoolName) : base(enemy, stateMachine, enemyData, animBoolName)
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
            enemy.gameObject.SetActive(false);
        }
        public override void AnimationEndTrigger()
        {
            base.AnimationEndTrigger();
        }
    }
}


