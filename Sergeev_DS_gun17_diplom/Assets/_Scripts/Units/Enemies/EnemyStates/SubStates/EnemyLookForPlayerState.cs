using UnityEngine;
using Metroidvania.BaseUnit;

namespace Metroidvania.Enemy
{
    public class EnemyLookForPlayerState : EnemyState
    {
        protected Movement Movement => _movement ? _movement : Unit.GetUnitComponent<Movement>(ref _movement);

        private CollisionChecks CollisionChecks => _collisionChecks
            ? _collisionChecks
            : Unit.GetUnitComponent<CollisionChecks>(ref _collisionChecks);

        private Movement _movement;
        private CollisionChecks _collisionChecks;
        private bool _turnNow;
        protected bool IsPlayerInMinAggroRange;
        protected bool IsAllTurnsDone;
        protected bool IsAllTimeDone;
        private float _lastTurnTime;
        private int _amountOfTurns;

        protected EnemyLookForPlayerState(BaseEnemy enemy, EnemyStateMachine stateMachine, EnemyData enemyData,
            string animBoolName) : base(enemy, stateMachine, enemyData, animBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();
            IsAllTimeDone = false;
            IsAllTurnsDone = false;
            _lastTurnTime = StartTime;
            _amountOfTurns = 0;
            Movement.SetVelocityZero();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (IsExitingState) return;
            Movement?.SetVelocityZero();
            if (_turnNow)
            {
                Turn();
                _turnNow = false;
            }
            else if (Time.time >= _lastTurnTime + EnemyData.timeOfTurns && !IsAllTurnsDone) Turn();

            if (_amountOfTurns >= EnemyData.amountOfTurns) IsAllTurnsDone = true;
            if (Time.time >= _lastTurnTime + EnemyData.timeOfTurns && IsAllTurnsDone)
            {
                IsAllTimeDone = true;
            }
        }

        protected override void DoChecks()
        {
            base.DoChecks();
            IsPlayerInMinAggroRange = Enemy.CheckPlayerInMinRange();
        }

        public void SetFlipNow(bool flip) => _turnNow = flip;

        private void Turn()
        {
            Movement?.Flip();
            _lastTurnTime = Time.time;
            _amountOfTurns++;
        }
    }
}