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
            get => _movement ?? Unit.GetUnitComponent<Movement>(ref _movement);
        }
        private CollisionChecks CollisionChecks
        {
            get => _collisionChecks ?? Unit.GetUnitComponent<CollisionChecks>(ref _collisionChecks);
        }
        private Movement _movement;
        private CollisionChecks _collisionChecks;
        protected bool TurnNow;
        protected bool IsPlayerInMinAggroRange;
        protected bool IsAllTurnsDone;
        protected bool IsAllTimeDone;
        protected float LastTurnTime;
        protected int AmountOfTurns;
        public EnemyLookForPlayerState(BaseEnemy enemy, EnemyStateMachine stateMachine, EnemyData enemyData, string animBoolName) : base(enemy, stateMachine, enemyData, animBoolName)
        {
        }
        public override void Enter()
        {
            base.Enter();
            IsAllTimeDone = false;
            IsAllTurnsDone = false;
            LastTurnTime = StartTime;
            AmountOfTurns = 0;
            Movement.SetVelocityZero();
        }
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (IsExitingState) return;
            Movement?.SetVelocityZero();
            if (TurnNow)
            {
                Turn();
                TurnNow = false;
            }
            else if (Time.time >= LastTurnTime + EnemyData.timeOfTurns && !IsAllTurnsDone) Turn();

            if (AmountOfTurns >= EnemyData.amountOfTurns) IsAllTurnsDone = true;
            if (Time.time >= LastTurnTime + EnemyData.timeOfTurns && IsAllTurnsDone)
            {
                IsAllTimeDone = true;
            }
        }
        public override void DoChecks()
        {
            base.DoChecks();
            IsPlayerInMinAggroRange = Enemy.CheckPlayerInMinRange();
        }
        public void SetFlipNow(bool flip) => TurnNow = flip;
        private void Turn()
        {
            Movement?.Flip();
            LastTurnTime = Time.time;
            AmountOfTurns++;
        }
    }
}

