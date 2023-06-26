using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Metroidvania.Structs;
using Metroidvania.BaseUnit;
using Metroidvania.Managers;
using UnityEngine.Serialization;

namespace Metroidvania.Enemy
{
    public class BaseEnemy : MonoBehaviour
    {
        protected EnemyStateMachine StateMachine { get; private set; }
        public Animator Animator { get; private set; }
        public AnimationToStateMachine AnimToStateMachine { get; private set; }
        public Vector2 CurrentVelocity { get; private set; }
        public Unit Unit { get; private set; }
        protected Movement Movement => _movement ? _movement : Unit.GetUnitComponent<Movement>(ref _movement);
        private Movement _movement;
        protected UnitStats UnitStats => _unitStats ? _unitStats : Unit.GetUnitComponent<UnitStats>(ref _unitStats);
        private UnitStats _unitStats;
        private Vector2 _workVector;
        [SerializeField] private Transform wallCheck;
        [SerializeField] private Transform ledgeCheck;
        [SerializeField] private Transform playerCheck;
        [SerializeField] protected EnemyData enemyData;
        [SerializeField] private LootType lootType;
        public AudioManager audioManager;
        
        private int _lastDamageDirection;
        
        public virtual void Awake()
        {
            StateMachine = new EnemyStateMachine();
            Animator = GetComponent<Animator>();
            AnimToStateMachine = GetComponent<AnimationToStateMachine>();
            Unit = GetComponentInChildren<Unit>();
        }
        protected virtual void Start()
        {
        }

        public virtual void Update()
        {
            Unit.LogicUpdate();
            StateMachine.CurrentState.LogicUpdate();
        }

        public LootType GetLootType() => lootType;
        public virtual void FixedUpdate()
        {
            StateMachine.CurrentState.PhysicsUpdate();
        }
        public virtual bool CheckPlayerInMinRange()
        {
            return Physics2D.Raycast(playerCheck.position, transform.right, enemyData.minAggroDistance, enemyData.playerLayer);
        }
        public virtual bool CheckPlayerInMaxRange()
        {
            return Physics2D.Raycast(playerCheck.position, transform.right, enemyData.maxAggroDistance, enemyData.playerLayer);
        }
        public virtual bool CheckPlayerInCloseRangeAction()
        {
            return Physics2D.Raycast(playerCheck.position, transform.right, enemyData.closeRangeActionDistance, enemyData.playerLayer);
        }
        public virtual bool CheckPlayerInLongRangeAction()
        {
            return Physics2D.Raycast(playerCheck.position, transform.right, enemyData.longRangeActionDistance, enemyData.playerLayer);
        }
        public virtual void OnDrawGizmos()
        {
            if (Unit == null) return;
            Gizmos.DrawLine(wallCheck.position, wallCheck.position + (Vector3)(Vector2.right * Movement.FacingDirection * enemyData.wallCheckDistance));
            Gizmos.DrawLine(ledgeCheck.position, ledgeCheck.position + (Vector3)(Vector2.down * enemyData.ledgeCheckDistance));
            Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * enemyData.closeRangeActionDistance), 0.2f);
            Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * enemyData.minAggroDistance), 0.2f);
            Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * enemyData.maxAggroDistance), 0.2f);
        }
    }
}

