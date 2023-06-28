using System;
using Metroidvania.Combat.Projectile;
using UnityEngine;
using UnityEngine.Pool;

namespace Metroidvania.Enemy
{
    public class BossEnemy : BaseEnemy
    {
        public BossIdleState IdleState { get; private set; }
        public BossTeleportInState TeleportInAboveState { get; private set; }
        public BossTeleportInState TeleportInLeftState { get; private set; }
        public BossTeleportInState TeleportInRightState { get; private set; }
        public BossTeleportInState TeleportInBehindState { get; private set; }
        public BossTeleportOutState TeleportOutState { get; private set; }
        public BossMeleeAttackState MeleeAttackState { get; private set; }
        public BossRangeAttackState RangeAttackState { get; private set; }

        public TeleportVariant lastTeleportVariant;
        public TeleportVariant beforeLastTeleportVariant;
        private IObjectPool<Projectile> _projectilePool;
        private Projectile _projectile;

        [SerializeField] private Transform meleeAttackPosition;

        [SerializeField] private Transform playerCheckPosition;

        [Header("Range attack")] [SerializeField]
        private Projectile projectilePrefab;

        [SerializeField] private Transform rangeAttackPosition;
        [SerializeField] private float attackPositionOffset;

        [Header("Spawn bats")] [SerializeField]
        private GameObject batPrefab;

        [SerializeField] private int numberOfBats;
        [SerializeField] private Vector2 batOffset;
        public event Action bossDied;

        public override void Awake()
        {
            base.Awake();
            _projectilePool = new ObjectPool<Projectile>(CreateProjectile, OnGetProjectile, OnReleaseProjectile);
            IdleState = new BossIdleState(this, StateMachine, enemyData, "idle", this);
            TeleportInAboveState = new BossTeleportInState(this, StateMachine, enemyData, "teleport_in", this,
                TeleportVariant.Above);
            TeleportInLeftState =
                new BossTeleportInState(this, StateMachine, enemyData, "teleport_in", this, TeleportVariant.Left);
            TeleportInRightState = new BossTeleportInState(this, StateMachine, enemyData, "teleport_in", this,
                TeleportVariant.Right);
            TeleportInBehindState = new BossTeleportInState(this, StateMachine, enemyData, "teleport_in", this,
                TeleportVariant.Behind);
            TeleportOutState = new BossTeleportOutState(this, StateMachine, enemyData, "teleport_out", this);
            MeleeAttackState =
                new BossMeleeAttackState(this, StateMachine, enemyData, "attack_2", meleeAttackPosition, this);
            RangeAttackState =
                new BossRangeAttackState(this, StateMachine, enemyData, "attack_1", rangeAttackPosition, this);
        }

        protected override void Start()
        {
            base.Start();
            StateMachine.Initialize(IdleState);
        }

        public override bool CheckPlayerInMaxRange()
        {
            var position = transform.position;
            return Physics2D.OverlapCircle(position, enemyData.maxAggroDistance, enemyData.playerLayer);
        }


        public Transform GetPlayerPosition()
        {
            if (!CheckPlayerInMaxRange()) return null;
            var position = transform.position;
            var overlapCircle = Physics2D.OverlapCircle(position, enemyData.maxAggroDistance, enemyData.playerLayer);
            return overlapCircle.transform;
        }

        public override void OnDrawGizmos()
        {
            Gizmos.DrawLine(playerCheckPosition.position, Vector2.right);
            Gizmos.DrawWireSphere(transform.position, enemyData.maxAggroDistance);
        }

        public void RangeAttack()
        {
            CreateProjectile();
        }

        public void SpawnBats()
        {
            for (var i = 0; i < numberOfBats; i++)
            {
                CreateBat(batOffset.x, batOffset.y);
            }
        }

        #region Range attack

        private Projectile CreateProjectile()
        {
            if (audioManager != null) audioManager.PlaySFX((int)SFXSlots.SwordThrow);
            Movement?.FlipToTarget(transform.position, GetPlayerPosition().position);
            var projectile = Instantiate(projectilePrefab,
                rangeAttackPosition.position + new Vector3(attackPositionOffset * Movement.FacingDirection, 0f, 0f),
                Quaternion.identity);
            projectile.SetupProjectile(GetPlayerPosition(), rangeAttackPosition, UnitStats.ArrowDamage());
            projectile.SetPool(_projectilePool);
            return projectile;
        }

        private void OnGetProjectile(Projectile obj)
        {
            obj.gameObject.SetActive(true);
            obj.transform.position =
                transform.position + new Vector3(attackPositionOffset * Movement.FacingDirection, 0f, 0f);
        }

        private void OnReleaseProjectile(Projectile obj)
        {
            obj.gameObject.SetActive(false);
        }

        #endregion

        #region Spawn bat

        private void CreateBat(float xOffset, float yOffset)
        {
            Movement?.FlipToTarget(transform.position, GetPlayerPosition().position);
            var bat = Instantiate(batPrefab,
                rangeAttackPosition.position + new Vector3((attackPositionOffset + xOffset) * Movement.FacingDirection,
                    yOffset, 0f),
                transform.rotation);
            if (audioManager != null) bat.GetComponent<BatEnemy>().audioManager = audioManager;
        }

        #endregion

        private void OnBossKilled()
        {
            bossDied?.Invoke();
        }

        private void OnEnable()
        {
            UnitStats.OnHealthZero += OnBossKilled;
        }

        protected void OnDisable()
        {
            UnitStats.OnHealthZero -= OnBossKilled;
        }
    }
}