using System;
using UnityEngine;

namespace Metroidvania.BaseUnit
{
    public class Death : UnitComponent
    {
        protected GameObject[] DeathParticles 
        { get => deathParticles; private set => deathParticles = value; }
        [SerializeField] private GameObject[] deathParticles;
        protected ParticleManager ParticleManager =>
            _particleManager ? _particleManager : Unit.GetUnitComponent<ParticleManager>(ref _particleManager);
        private ParticleManager _particleManager;
        private UnitStats UnitStats => _unitStats ? _unitStats : Unit.GetUnitComponent<UnitStats>(ref _unitStats);
        private UnitStats _unitStats;
        
        protected virtual void Die()
        {
            foreach (var particle in DeathParticles)
            {
                ParticleManager.StartParticle(particle);
            }
        }
        private void OnEnable()
        {
            UnitStats.OnHealthZero += Die;
        }
        private void OnDisable()
        {
            UnitStats.OnHealthZero -= Die;
        }

    }
}

