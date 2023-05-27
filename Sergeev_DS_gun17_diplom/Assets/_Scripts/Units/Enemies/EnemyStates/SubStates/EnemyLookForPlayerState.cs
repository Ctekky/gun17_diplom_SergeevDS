using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Metroidvania.BaseUnit;
namespace Metroidvania.Enemy
{
    public class EnemyLookForPlayerState : EnemyState
    {
        protected Movement Movement
        {
            get => movement ?? unit.GetUnitComponent<Movement>(ref movement);
        }
        private CollisionChecks CollisionChecks
        {
            get => collisionChecks ?? unit.GetUnitComponent<CollisionChecks>(ref collisionChecks);
        }
        private Movement movement;
        private CollisionChecks collisionChecks;
        protected bool turnNow;
        protected bool isPlayerInMinAggroRange;
        protected bool isAllTurnsDone;
        protected bool isAllTimeDone;
        protected float lastTurnTime;
        protected int amountOfTurns;
        public EnemyLookForPlayerState(BaseEnemy enemy, EnemyStateMachine stateMachine, EnemyData enemyData, string animBoolName) : base(enemy, stateMachine, enemyData, animBoolName)
        {
        }
        public override void Enter()
        {
            base.Enter();
            isAllTimeDone = false;
            isAllTurnsDone = false;
            lastTurnTime = startTime;
            amountOfTurns = 0;
            Movement.SetVelocityZero();
        }
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (isExitingState) return;
            Movement?.SetVelocityZero();
            if (turnNow)
            {
                Turn();
                turnNow = false;
            }
            else if (Time.time >= lastTurnTime + enemyData.timeOfTurns && !isAllTurnsDone) Turn();

            if (amountOfTurns >= enemyData.amountOfTurns) isAllTurnsDone = true;
            if (Time.time >= lastTurnTime + enemyData.timeOfTurns && isAllTurnsDone)
            {
                isAllTimeDone = true;
            }
        }
        public override void DoChecks()
        {
            base.DoChecks();
            isPlayerInMinAggroRange = enemy.CheckPlayerInMinRange();
        }
        public void SetFlipNow(bool flip) => turnNow = flip;
        private void Turn()
        {
            Movement?.Flip();
            lastTurnTime = Time.time;
            amountOfTurns++;
        }
    }
}

