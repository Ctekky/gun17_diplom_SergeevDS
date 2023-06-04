using Metroidvania.BaseUnit;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;

namespace Metroidvania.Combat.Weapon
{
    public class RangedWeapon : Weapon
    {
        private Movement Movement => _movement ? _movement : Unit.GetUnitComponent<Movement>(ref _movement);
        private Movement _movement;
        protected DamageWeaponData DamageWeaponData;
        [SerializeField] private Projectile.Projectile projectilePrefab;
        [SerializeField] private float attackPositionOffset;
        [SerializeField] protected Transform player;
        private IObjectPool<Projectile.Projectile> _projectilePool;
        private bool _isSimpleDirection;
        private Vector2 _finalDirection;

        [Header("Aim")]
        [SerializeField] private int numberOfDots;
        [SerializeField] private float spaceBetweenDots;
        [SerializeField] private GameObject dotPrefab;
        [SerializeField] private Transform dotsParent;
        [SerializeField] private float gravityScale;
        [SerializeField] private Vector2 launchForce = new Vector2(15f, 15f);
        private GameObject[] _dots;
        
        protected override void Awake()
        {
            base.Awake();
            if (weaponData.GetType() == typeof(DamageWeaponData)) DamageWeaponData = (DamageWeaponData)weaponData;
            else Debug.LogError("Wrong data for weapon");
            _projectilePool = new ObjectPool<Projectile.Projectile>(CreateProjectile, OnGetProjectile, OnReleaseProjectile);
            _isSimpleDirection = true;
            GenerateDots();
        }
        private Projectile.Projectile CreateProjectile()
        {
            var projectile = Instantiate(projectilePrefab, player.position + new Vector3(attackPositionOffset * Movement.FacingDirection, 0f, 0f), player.rotation);
            _finalDirection = new Vector2(AimDirection().normalized.x * launchForce.x,
                AimDirection().normalized.y * launchForce.y);
            if (_isSimpleDirection) projectile.SetupProjectile(UnitStats.ArrowDamage());
            else projectile.SetupProjectile(_finalDirection, UnitStats.ArrowDamage());
            projectile.SetPool(_projectilePool);
            return projectile;
        }
        private void OnGetProjectile(Projectile.Projectile obj)
        {
            obj.gameObject.SetActive(true);
            obj.transform.position = transform.position + new Vector3(attackPositionOffset * Movement.FacingDirection, 0f, 0f);

        }
        private void OnReleaseProjectile(Projectile.Projectile obj)
        {
            obj.gameObject.SetActive(false);
        }

        public override void AnimationActionTrigger()
        {
            base.AnimationActionTrigger();
            RangeAttack();
        }
        private void RangeAttack()
        {
            _isSimpleDirection = true;
            CreateProjectile();
        }
        private void SecondaryRangeAttack()
        {
            _isSimpleDirection = false;
            CreateProjectile();
        }
        public override void EnterWeaponSecondary()
        {
            base.EnterWeaponSecondary();
            DotsActive(false);
            SecondaryRangeAttack();
        }

        public override void EnterWeaponAim()
        {
            base.EnterWeaponAim();
            DotsActive(true);
            ShowDots();
            
        }
        private Vector2 AimDirection()
        {
            Vector2 playerPosition = player.position;
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = mousePosition - playerPosition;
            return direction;
        }

        private void DotsActive(bool isActive)
        {
            for (int i = 0; i < _dots.Length; i++)
            {
                _dots[i].SetActive(isActive);
            }
        }
        private void GenerateDots()
        {
            _dots = new GameObject[numberOfDots];
            for (int i = 0; i < numberOfDots; i++)
            {
                _dots[i] = Instantiate(dotPrefab, player.position, Quaternion.identity, dotsParent);
                _dots[i].SetActive(false);
            }
        }

        private Vector2 DotsPosition(float t)
        {
            var position = (Vector2)player.position + new Vector2(
                    AimDirection().normalized.x * launchForce.x, AimDirection().normalized.y * launchForce.y)
                * t + .5f * (Physics2D.gravity * gravityScale) * (t * t);
            return position;
        }

        private void ShowDots()
        {
            for (int i = 0; i < _dots.Length; i++)
            {
                _dots[i].transform.position = DotsPosition(i * spaceBetweenDots);
                //Debug.Log($"Point {i.ToString()} have coord {_dots[i].transform.position.x.ToString()} and {_dots[i].transform.position.y.ToString()}");
            }
        }
    }
}

