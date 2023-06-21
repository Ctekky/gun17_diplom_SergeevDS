using System;
using Metroidvania.Interfaces;
using UnityEngine;

namespace Metroidvania.Combat
{
    public class DamageGround : MonoBehaviour
    {
        [SerializeField] private int damage;

        private void OnCollisionEnter2D(Collision2D other)
        {
            if(other.transform.GetComponentInChildren<IDamageable>() != null)
                other.transform.GetComponentInChildren<IDamageable>().Damage(damage);
        }
    }
}
