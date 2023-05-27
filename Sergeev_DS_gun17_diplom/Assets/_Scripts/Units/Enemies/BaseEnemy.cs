using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Metroidvania.Structs;
using Metroidvania.BaseUnit;

namespace Metroidvania.Enemy
{
    public class BaseEnemy : MonoBehaviour
    {
        public EnemyStateMachine StateMachine { get; private set; }
        public Animator Animator { get; private set; }
        public AnimationToStateMachine animToStateMachine { get; private set; }
        public Vector2 CurrentVelocity { get; private set; }
        public Unit Unit { get; private set; }

        protected Movement Movement
        {
            get => movement ?? Unit.GetUnitComponent<Movement>(ref movement);
        }
        private Movement movement;
        private Vector2 workVector;
        [SerializeField]
        private Transform wallCheck;
        [SerializeField]
        private Transform ledgeCheck;
        [SerializeField]
        private Transform playerCheck;
        [SerializeField]
        protected EnemyData enemyData;
        private float currentHealth;
        private int lastDamageDirection;


        public virtual void Awake()
        {
            StateMachine = new EnemyStateMachine();
            Animator = GetComponent<Animator>();
            animToStateMachine = GetComponent<AnimationToStateMachine>();
            Unit = GetComponentInChildren<Unit>();
        }

        public virtual void Start()
        {
            currentHealth = enemyData.health;
        }
        public virtual void Update()
        {
            Unit.LogicUpdate();
            StateMachine.CurrentState.LogicUpdate();
        }
        public virtual void FixedUpdate()
        {
            StateMachine.CurrentState.PhysicsUpdate();
        }
        public virtual void DamageHop(float velocity)
        {
            workVector.Set(Movement.RB.velocity.x, velocity);
            Movement.RB.velocity = workVector;
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
            if(Unit != null)
            {
                Gizmos.DrawLine(wallCheck.position, wallCheck.position + (Vector3)(Vector2.right * Movement.FacingDirection * enemyData.wallCheckDistance));
                Gizmos.DrawLine(ledgeCheck.position, ledgeCheck.position + (Vector3)(Vector2.down * enemyData.ledgeCheckDistance));

                Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * enemyData.closeRangeActionDistance), 0.2f);
                Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * enemyData.minAggroDistance), 0.2f);
                Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * enemyData.maxAggroDistance), 0.2f);
            }

        }
    }
}

