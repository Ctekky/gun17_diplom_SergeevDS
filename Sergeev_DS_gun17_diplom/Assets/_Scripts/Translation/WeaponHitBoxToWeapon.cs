using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Metroidvania.Combat.Weapon
{
    public class WeaponHitBoxToWeapon : MonoBehaviour
    {
        private DamageWeapon weapon;

        private void Awake()
        {
            weapon = GetComponentInParent<DamageWeapon>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            weapon.AddToDetected(collision);
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            weapon.RemoveFromDetected(collision);
        }
    }
}

