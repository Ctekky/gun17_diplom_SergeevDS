using Metroidvania.Combat.Projectile;
using UnityEngine;
using UnityEngine.Pool;

namespace Metroidvania.Enemy
{
    public class SkeletonArcherEnemy : BaseEnemy
    {
        public SkeletonArcherIdleState IdleState { get; private set; }
        public SkeletonArcherAttackState AttackState { get; private set; }
        public SkeletonArcherLookForPlayerState LookForPlayerState { get; private set; }
        [SerializeField] private Transform attackPosition;
        [SerializeField] private Transform playerCheckPosition;
        [SerializeField] private Projectile projectilePrefab;
        [SerializeField] private float attackPositionOffset;
        [SerializeField] private Vector2 launchForce = new Vector2(15f, 15f);

        private IObjectPool<Projectile> _projectilePool;
        private bool _isSimpleDirection;
        private Vector2 _finalDirection;
        private Projectile _projectile;

        public override void Awake()
        {
            base.Awake();
            _projectilePool = new ObjectPool<Projectile>(CreateProjectile, OnGetProjectile, OnReleaseProjectile);
            IdleState = new SkeletonArcherIdleState(this, StateMachine, enemyData, "idle", this);
            LookForPlayerState = new SkeletonArcherLookForPlayerState(this, StateMachine, enemyData, "idle", this);
            AttackState = new SkeletonArcherAttackState(this, StateMachine, enemyData, "attack",
                attackPosition, this);
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
            _isSimpleDirection = true;
            CreateProjectile();
        }

        private Projectile CreateProjectile()
        {
            if(audioManager != null) audioManager.PlaySFX((int)SFXSlots.SwordThrow);
            var projectile = Instantiate(projectilePrefab,
                attackPosition.position + new Vector3(attackPositionOffset * Movement.FacingDirection, 0f, 0f),
                transform.rotation);
            _finalDirection = new Vector2(GetPlayerPosition().position.normalized.x * launchForce.x,
                GetPlayerPosition().position.normalized.y * launchForce.y);
            if (_isSimpleDirection) projectile.SetupProjectile(UnitStats.ArrowDamage());
            else projectile.SetupProjectile(_finalDirection, UnitStats.ArrowDamage());
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
    }
}