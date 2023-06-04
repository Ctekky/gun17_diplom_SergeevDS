using System;
using UnityEngine;
using Metroidvania.BaseUnit;

namespace Metroidvania.Enemy
{
    public class EnemyDeathUnitComponent : Death
    {
        public event Action<Vector2> OnDied;
        protected override void Die()
        {
            foreach (var particle in DeathParticles)
            {
                var transform1 = Unit.transform;
                ParticleManager.StartParticle(particle, transform1.position, transform1.rotation);
            }
            var parent = Unit.transform.parent;
            OnDied?.Invoke(parent.transform.position);
            parent.gameObject.SetActive(false);
        }
    }   
}

