using Metroidvania.Structs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Metroidvania.Combat.Weapon
{
    [CreateAssetMenu(fileName = "New daamge weapon data", menuName = "Data/Weapon data/Damage Weapon")]
    public class DamageWeaponData : WeapondData
    {
        [SerializeField] private WeaponAttackDetails[] attackDetails;
        public WeaponAttackDetails[] AttackDetails { get => attackDetails; private set => attackDetails = value; }

        private void OnEnable()
        {
            AmountOfAttacks = attackDetails.Length;
            MovementSpeed = new float[AmountOfAttacks];
            for (int i = 0; i < AmountOfAttacks; i++)
            {
                MovementSpeed[i] = attackDetails[i].movementSpeed;
            }
        }
    }

}
