using Metroidvania.Interfaces;
using System.Collections;
using UnityEngine;

namespace Metroidvania.BaseUnit
{
    public class Combat : UnitComponent, IDamageable, IKnockbackable
    {
        [SerializeField] private float maxKnockbackTime = 0.2f;
        [SerializeField] private GameObject damageParticle;
        [SerializeField] private Material hitMaterial;
        [SerializeField] private float flashDuration;
        private Movement Movement => _movement ? _movement : Unit.GetUnitComponent<Movement>(ref _movement);
        private CollisionChecks CollisionChecks => _collisionChecks ? _collisionChecks : Unit.GetUnitComponent<CollisionChecks>(ref _collisionChecks);
        private UnitStats UnitStats => _unitStats ? _unitStats : Unit.GetUnitComponent<UnitStats>(ref _unitStats);
        private ParticleManager ParticleManager => _particleManager ? _particleManager : Unit.GetUnitComponent<ParticleManager>(ref _particleManager);
        private UnitStats _unitStats;
        private Movement _movement;
        private CollisionChecks _collisionChecks;
        private ParticleManager _particleManager;
        private bool _isKnockbackActive;
        private float _knockbackStartTime;
        private SpriteRenderer _sr;
        private Material _originalMaterial;
        private bool _isImmune;
        protected override void Awake()
        {
            base.Awake();
            _sr = GetComponentInParent<SpriteRenderer>();
        }
        protected override void Start()
        {
            _originalMaterial = _sr.material;
            _isImmune = false;
        }
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            CheckKnockback();
        }
        public void Damage(int amount)
        {
            if(_isImmune) return;
            StartCoroutine(nameof(FlashFX));
            UnitStats?.DecreaseHealth(amount);
            ParticleManager?.StartParticleWithRandomRotation(damageParticle);
        }
        public void Knockback(Vector2 angle, float strength, int direction)
        {
            Movement?.SetVelocity(strength, angle, direction);
            Movement.CanSetVelocity = false;
            _isKnockbackActive = true;
            _knockbackStartTime = Time.time;
        }
        private void CheckKnockback()
        {
            if (_isKnockbackActive && ((Movement?.CurrentVelocity.y <= 0.01f && CollisionChecks.Grounded) || Time.time >= _knockbackStartTime + maxKnockbackTime))
            {
                _isKnockbackActive = false;
                Movement.CanSetVelocity = true;
            }
        }
        private IEnumerator FlashFX()
        {
            _isImmune = true;
            _sr.material = hitMaterial;
            yield return new WaitForSeconds(flashDuration);
            _sr.material = _originalMaterial;
            _isImmune = false;
        }
    }
}


