using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Metroidvania.Structs;

namespace Metroidvania.Enemy
{
    public class BaseEnemy : MonoBehaviour
    {
        public EnemyStateMachine StateMachine { get; private set; }
        public Animator Animator { get; private set; }
        public Rigidbody2D RB { get; private set; }
        public GameObject AliveGO { get; private set; }
        public AnimationToStateMachine animToStateMachine { get; private set; }
        public Vector2 CurrentVelocity { get; private set; }
        public int FacingDirection { get; private set; }
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
        }

        public virtual void Start()
        {
            AliveGO = transform.Find("Alive").gameObject;
            RB = AliveGO.GetComponent<Rigidbody2D>();
            Animator = AliveGO.GetComponent<Animator>();
            animToStateMachine = AliveGO.GetComponent<AnimationToStateMachine>();
            FacingDirection = 1;
            currentHealth = enemyData.health;
        }
        public virtual void Update()
        {
            StateMachine.CurrentState.LogicUpdate();
        }
        public virtual void FixedUpdate()
        {
            StateMachine.CurrentState.PhysicsUpdate();
        }
        public virtual void SetVelocity(float velocity)
        {
            workVector.Set(FacingDirection * velocity, RB.velocity.y);
            RB.velocity = workVector;
        }
        public void SetVelocityZero()
        {
            RB.velocity = Vector2.zero;
            CurrentVelocity = Vector2.zero;
        }
        public virtual void DamageHop(float velocity)
        {
            workVector.Set(RB.velocity.x, velocity);
            RB.velocity = workVector;
        }
        public virtual void GetDamage(AttackDetails attackDetails)
        {
            currentHealth -= attackDetails.damage;
            DamageHop(enemyData.damageHopSpeed);
            if (attackDetails.position.x > AliveGO.transform.position.x)
            {
                lastDamageDirection = -1;
            }
            else
            {
                lastDamageDirection = 1;
            }
        }
        public virtual bool CheckWall()
        {
            return Physics2D.Raycast(wallCheck.position, AliveGO.transform.right * FacingDirection, enemyData.wallCheckDistance, enemyData.groundLayer);
        }
        public virtual bool CheckLedge()
        {
            return Physics2D.Raycast(ledgeCheck.position, Vector2.down, enemyData.ledgeCheckDistance, enemyData.groundLayer);
        }
        public virtual bool CheckPlayerInMinRange()
        {
            return Physics2D.Raycast(playerCheck.position, AliveGO.transform.right, enemyData.minAggroDistance, enemyData.playerLayer);
        }
        public virtual bool CheckPlayerInMaxRange()
        {
            return Physics2D.Raycast(playerCheck.position, AliveGO.transform.right, enemyData.maxAggroDistance, enemyData.playerLayer);
        }
        public virtual bool CheckPlayerInCloseRangeAction()
        {
            return Physics2D.Raycast(playerCheck.position, AliveGO.transform.right, enemyData.closeRangeActionDistance, enemyData.playerLayer);
        }
        public virtual bool CheckPlayerInLongRangeAction()
        {
            return Physics2D.Raycast(playerCheck.position, AliveGO.transform.right, enemyData.longRangeActionDistance, enemyData.playerLayer);
        }
        public virtual void Flip()
        {
            FacingDirection *= -1;
            AliveGO.transform.Rotate(0f, 180f, 0f);
        }

        public virtual void OnDrawGizmos()
        {
            Gizmos.DrawLine(wallCheck.position, wallCheck.position + (Vector3)(Vector2.right * FacingDirection * enemyData.wallCheckDistance));
            Gizmos.DrawLine(ledgeCheck.position, ledgeCheck.position + (Vector3)(Vector2.down * enemyData.ledgeCheckDistance));

            Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * enemyData.closeRangeActionDistance), 0.2f);
            Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * enemyData.minAggroDistance), 0.2f);
            Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * enemyData.maxAggroDistance), 0.2f);
        }
    }
}

