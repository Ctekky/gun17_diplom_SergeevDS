using UnityEngine;

namespace Metroidvania.BaseUnit
{
    public class Death : UnitComponent
    {
        protected GameObject[] DeathParticles { get => deathParticles; private set { deathParticles = value; } }
        [SerializeField] private GameObject[] deathParticles;
        
        protected ParticleManager ParticleManager =>
            particleManager ? particleManager : unit.GetUnitComponent<ParticleManager>(ref particleManager);
        private ParticleManager particleManager;
        protected Stats Stats => stats ? stats : unit.GetUnitComponent<Stats>(ref stats);
        private Stats stats;
        protected virtual void Die()
        {
            foreach (var particle in DeathParticles)
            {
                ParticleManager.StartParticle(particle);
            }
            unit.transform.parent.gameObject.SetActive(false);
        }
        private void OnEnable()
        {
            Stats.OnHealthZero += Die;
        }
        private void OnDisable()
        {
            Stats.OnHealthZero -= Die;
        }

    }
}

