using Metroidvania.Interfaces;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Metroidvania.BaseUnit;
namespace Metroidvania.Combat.Weapon
{
    public class DamageWeapon : Weapon
    {
        private Movement Movement => _movement ? _movement : Unit.GetUnitComponent<Movement>(ref _movement);
        private Movement _movement;
        private readonly List<IDamageable> _detectedDamageables = new List<IDamageable>();
        private readonly List<IKnockbackable> _detectedKnockbackables = new List<IKnockbackable>();
        private DamageWeaponData _damageWeaponData;
        protected override void Awake()
        {
            base.Awake();
            if (weaponData.GetType() == typeof(DamageWeaponData)) _damageWeaponData = (DamageWeaponData)weaponData;
            else Debug.LogError("Wrong data for weapon");
        }

        public override void EnterWeapon()
        {
            base.EnterWeapon();
            Player.audioManager.PlaySFX((int)SFXSlots.SwordAttackWind);
        }

        public override void AnimationActionTrigger()
        {
            base.AnimationActionTrigger();
            CheckMeleeAttack();
        }
        private void CheckMeleeAttack()
        {
            var attackDetails = _damageWeaponData.AttackDetails[AttackCounter];
            foreach (var item in _detectedDamageables.ToList())
            {
                var finalDamage = UnitStats.DoDamage(attackDetails.damageAmount.GetValue());
                item.Damage(finalDamage);
            }
            foreach (var item in _detectedKnockbackables.ToList())
            {
                item.Knockback(attackDetails.knockbackAngle, attackDetails.knockbackStrength, Movement.FacingDirection);
            }
        }
        public void AddToDetected(Collider2D collision)
        {
            var damageable = collision.GetComponentInParent<IDamageable>();
            if (damageable != null)
            {
                _detectedDamageables.Add(damageable);
            }
            var knockbackable = collision.GetComponentInParent<IKnockbackable>();
            if(knockbackable != null)
            {
                _detectedKnockbackables.Add(knockbackable);
            }
        }
        public void RemoveFromDetected(Collider2D collision)
        {
            var damageable = collision.GetComponentInParent<IDamageable>();
            if (damageable != null)
            {
                _detectedDamageables.Remove(damageable);
            }
            var knockbackable = collision.GetComponentInParent<IKnockbackable>();
            if (knockbackable != null)
            {
                _detectedKnockbackables.Remove(knockbackable);
            }
        }
    }
}

