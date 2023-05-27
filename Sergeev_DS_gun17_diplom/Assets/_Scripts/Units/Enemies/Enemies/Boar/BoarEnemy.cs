using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Metroidvania.Interfaces;

namespace Metroidvania.Enemy
{
    public class BoarEnemy : BaseEnemy
    {
        public BoarIdleState IdleState { get; private set; }
        public BoarMoveState MoveState { get; private set; }
        public BoarDetectedPlayerState DetectedPlayerState { get; private set; }
        public BoarChargeState ChargeState { get; private set; }
        public BoarLookForPlayerState LookForPlayerState { get; private set; }
        public BoarMeleeAttackState MeleeAttackState { get; private set; }

        [SerializeField]
        private Transform meleeAttackPostion;
        public override void Awake()
        {
            base.Awake();
            IdleState = new BoarIdleState(this, StateMachine, enemyData, "idle", this);
            MoveState = new BoarMoveState(this, StateMachine, enemyData, "move", this);
            DetectedPlayerState = new BoarDetectedPlayerState(this, StateMachine, enemyData, "detectedPlayer", this);
            ChargeState = new BoarChargeState(this, StateMachine, enemyData, "charge", this);
            LookForPlayerState = new BoarLookForPlayerState(this, StateMachine, enemyData, "idle", this);
            MeleeAttackState = new BoarMeleeAttackState(this, StateMachine, enemyData, "meleeAttack", meleeAttackPostion, this);
        }
        public override void Start()
        {
            base.Start();
            StateMachine.Initialize(MoveState);
        }

        public override void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            Gizmos.DrawWireSphere(meleeAttackPostion.position, enemyData.meleeAttackRadius);
        }
        private void OnDeathAnimationEnd()
        {
            gameObject.SetActive(false);
        }

    }
}

