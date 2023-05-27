using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Metroidvania.BaseUnit;

namespace Metroidvania.Enemy
{
    public class EnemyGroundedState : EnemyState
    {
        protected Movement Movement
        {
            get => movement ?? unit.GetUnitComponent<Movement>(ref movement);
        }
        protected CollisionChecks CollisionChecks
        {
            get => collisionChecks ?? unit.GetUnitComponent<CollisionChecks>(ref collisionChecks);
        }
        private Movement movement;
        private CollisionChecks collisionChecks;
        protected bool isPlayerInMinAggroRange;
        public EnemyGroundedState(BaseEnemy enemy, EnemyStateMachine stateMachine, EnemyData enemyData, string animBoolName) : base(enemy, stateMachine, enemyData, animBoolName)
        {
        }
        public override void DoChecks()
        {
            base.DoChecks();
            isPlayerInMinAggroRange = enemy.CheckPlayerInMinRange();
        }
    }

}

