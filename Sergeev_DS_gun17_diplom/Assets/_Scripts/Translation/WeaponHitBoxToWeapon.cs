using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Metroidvania.Combat.Weapon
{
    public class WeaponHitBoxToWeapon : MonoBehaviour
    {
        private DamageWeapon _weapon;

        private void Awake()
        {
            _weapon = GetComponentInParent<DamageWeapon>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            _weapon.AddToDetected(collision);
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            _weapon.RemoveFromDetected(collision);
        }
    }
}

