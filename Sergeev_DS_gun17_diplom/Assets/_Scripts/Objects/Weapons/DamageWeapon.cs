using Metroidvania.Interfaces;
using Metroidvania.Structs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Metroidvania.BaseUnit;
namespace Metroidvania.Combat.Weapon
{
    public class DamageWeapon : Weapon
    {
        protected Movement Movement => movement ? movement : unit.GetUnitComponent<Movement>(ref movement);
        private Movement movement;
        private List<IDamageable> detectedDamageables = new List<IDamageable>();
        private List<IKnockbackable> detectedKnockbackables = new List<IKnockbackable>();
        protected DamageWeaponData damageWeaponData;
        protected override void Awake()
        {
            base.Awake();
            if (weapondData.GetType() == typeof(DamageWeaponData)) damageWeaponData = (DamageWeaponData)weapondData;
            else Debug.LogError("Wrong data for weapon");
        }
        public override void AnimationActionTrigger()
        {
            base.AnimationActionTrigger();
            CheckMeleeAttack();
        }
        private void CheckMeleeAttack()
        {
            WeaponAttackDetails attackDetails = damageWeaponData.AttackDetails[attackCounter];
            foreach (IDamageable item in detectedDamageables.ToList())
            {
                item.Damage(attackDetails.damageAmount);
            }
            foreach (var item in detectedKnockbackables.ToList())
            {
                item.Knockback(attackDetails.knockbackAngle, attackDetails.knockbackStrength, Movement.FacingDirection);
            }
        }
        public void AddToDetected(Collider2D collision)
        {
            IDamageable damageable = collision.GetComponentInParent<IDamageable>();
            if (damageable != null)
            {
                detectedDamageables.Add(damageable);
            }
            IKnockbackable knockbackable = collision.GetComponentInParent<IKnockbackable>();
            if(knockbackable != null)
            {
                detectedKnockbackables.Add(knockbackable);
            }
        }
        public void RemoveFromDetected(Collider2D collision)
        {
            IDamageable damageable = collision.GetComponentInParent<IDamageable>();
            if (damageable != null)
            {
                detectedDamageables.Remove(damageable);
            }
            IKnockbackable knockbackable = collision.GetComponentInParent<IKnockbackable>();
            if (knockbackable != null)
            {
                detectedKnockbackables.Remove(knockbackable);
            }
        }
    }
}

