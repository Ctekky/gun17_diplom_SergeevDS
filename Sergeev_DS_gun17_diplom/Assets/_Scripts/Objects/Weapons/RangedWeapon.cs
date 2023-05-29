using Metroidvania.BaseUnit;
using Metroidvania.Interfaces;
using Metroidvania.Structs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Metroidvania.Combat.Projectile;

using UnityEngine.Pool;

namespace Metroidvania.Combat.Weapon
{
    public class RangedWeapon : Weapon
    {
        protected Movement Movement => movement ? movement : unit.GetUnitComponent<Movement>(ref movement);
        private Movement movement;
        protected DamageWeaponData damageWeaponData;
        [SerializeField] private Projectile.Projectile projectilePrefab;
        [SerializeField] private float attackPositionOffset;
        private IObjectPool<Projectile.Projectile> projectilePool;
        private bool isSimpleDirection;
        private Vector2 direction = new Vector2(15f, 15f);

        protected override void Awake()
        {
            base.Awake();
            if (weapondData.GetType() == typeof(DamageWeaponData)) damageWeaponData = (DamageWeaponData)weapondData;
            else Debug.LogError("Wrong data for weapon");
            projectilePool = new ObjectPool<Projectile.Projectile>(CreateProjectile, OnGetProjectile, OnReleaseProjectile);
            isSimpleDirection = true;
        }
        private Projectile.Projectile CreateProjectile()
        {
            Projectile.Projectile projectile = Instantiate(projectilePrefab, transform.position + new Vector3(attackPositionOffset * Movement.FacingDirection, 0f, 0f), transform.rotation);
            if (isSimpleDirection) projectile.SetupProjectile();
            else projectile.SetupProjectile(direction);
            projectile.SetPool(projectilePool);
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
            isSimpleDirection = true;
            CreateProjectile();
        }
        private void SecondaryRangeAttack()
        {
            isSimpleDirection = false;
            CreateProjectile();
        }
        public override void EnterWeaponSecondary()
        {
            base.EnterWeaponSecondary();
            SecondaryRangeAttack();
        }
    }
}

