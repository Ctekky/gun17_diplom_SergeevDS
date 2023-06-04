using UnityEngine;

namespace Metroidvania.BaseUnit
{
    public class ParticleManager : UnitComponent
    {
        private Transform _particleContainer;
        protected override void Awake()
        {
            base.Awake();
            _particleContainer = GameObject.FindGameObjectWithTag("ParticleContainer").transform;
        }
        public GameObject StartParticle(GameObject particlePrefab, Vector2 position, Quaternion rotation) => Instantiate(particlePrefab, position, rotation, _particleContainer);
        public GameObject StartParticle(GameObject particlePrefab) => StartParticle(particlePrefab, transform.position, Quaternion.identity);
        public GameObject StartParticleWithRandomRotation(GameObject particlePrefab)
        {
            var randomRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
            return StartParticle(particlePrefab, transform.position, randomRotation);
        }
    }
}

