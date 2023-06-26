namespace Metroidvania.Enemy
{
    public class BoarChargeState : EnemyChargeState
    {
        private readonly BoarEnemy _boarEnemy;
        public BoarChargeState(BaseEnemy enemy, EnemyStateMachine stateMachine, EnemyData enemyData, string animBoolName, BoarEnemy boarEnemy) : base(enemy, stateMachine, enemyData, animBoolName)
        {
            _boarEnemy = boarEnemy;
        }

        public override void Enter()
        {
            base.Enter();
            Enemy.audioManager.PlaySFX((int)SFXSlots.MonsterGrowl1);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (IsDetectingWall || !IsDetectingLedge)
            {
                StateMachine.ChangeState(_boarEnemy.LookForPlayerState);
            }
            else if(IsChargeTimeOver || PerformCloseRangeAction)
            {
                if(PerformCloseRangeAction)
                {
                    StateMachine.ChangeState(_boarEnemy.MeleeAttackState);
                }    
                else if(IsPlayerInMinAggroRange)
                {
                    StateMachine.ChangeState(_boarEnemy.DetectedPlayerState);
                }
            }
        }
    }
}

