using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Metroidvania.BaseUnit;

namespace Metroidvania.Player
{
    public class PlayerDeathUnitComponent : Death
    {
        protected override void Die()
        {
            foreach (var particle in DeathParticles)
            {
                ParticleManager.StartParticle(particle, Unit.transform.position, Unit.transform.rotation);
            }
            //Unit.transform.parent.gameObject.SetActive(false);
        }

    }
}

