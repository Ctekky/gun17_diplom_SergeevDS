using Metroidvania.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Metroidvania.BaseUnit
{
    public class Combat : UnitComponent, IDamageable, IKnockbackable
    {
        [SerializeField] private float maxKnockbackTime = 0.2f;
        [SerializeField] private GameObject damageParticle;
        [SerializeField] private Material hitMaterial;
        [SerializeField] private float flashDuration;
        protected Movement Movement => movement ? movement : unit.GetUnitComponent<Movement>(ref movement);
        private CollisionChecks CollisionChecks => collisionChecks ? collisionChecks : unit.GetUnitComponent<CollisionChecks>(ref collisionChecks);
        private Stats Stats => stats ? stats : unit.GetUnitComponent<Stats>(ref stats);
        private ParticleManager ParticleManager => particleManager ? particleManager : unit.GetUnitComponent<ParticleManager>(ref particleManager);
        private Stats stats;
        private Movement movement;
        private CollisionChecks collisionChecks;
        private ParticleManager particleManager;
        private bool isKnockbackActive;
        private float knockbackStartTime;
        private SpriteRenderer sr;
        private Material originalMaterial;
        private bool isImmune;
        protected override void Awake()
        {
            base.Awake();
            sr = GetComponentInParent<SpriteRenderer>();
        }
        protected override void Start()
        {
            originalMaterial = sr.material;
            isImmune = false;
        }
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            CheckKnockback();
        }
        public void Damage(float amount)
        {
            if(isImmune) return;
            Debug.Log(unit.transform.parent.name + " damaged!");
            StartCoroutine("FlashFX");
            Stats?.DecreaseHealth(amount);
            ParticleManager?.StartParticleWithRandomRotation(damageParticle);
        }
        public void Knockback(Vector2 angle, float strength, int direction)
        {
            Movement?.SetVelocity(strength, angle, direction);
            Movement.CanSetVelocity = false;
            isKnockbackActive = true;
            knockbackStartTime = Time.time;
        }
        private void CheckKnockback()
        {
            if (isKnockbackActive && ((Movement?.CurrentVelocity.y <= 0.01f && CollisionChecks.Grounded) || Time.time >= knockbackStartTime + maxKnockbackTime))
            {
                isKnockbackActive = false;
                Movement.CanSetVelocity = true;
            }
        }
        IEnumerator FlashFX()
        {
            isImmune = true;
            sr.material = hitMaterial;
            yield return new WaitForSeconds(flashDuration);
            sr.material = originalMaterial;
            isImmune = false;
        }
    }
}


