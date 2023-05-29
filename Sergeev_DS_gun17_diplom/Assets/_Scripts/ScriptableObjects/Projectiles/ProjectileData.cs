using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Metroidvania.Combat.Projectile
{
    [CreateAssetMenu(fileName = "New projectile data", menuName = "Data/Projectile data/Projectile")]
    public class ProjectileData : ScriptableObject
    {
        [SerializeField] public float speed;
        [SerializeField] public float travelDistance;
        [SerializeField] public float damage;
        [SerializeField] public float lifeTime;
        [SerializeField] public bool isRopeArrow;
        [SerializeField] public GameObject arrowRopePrefab;
    }
}


