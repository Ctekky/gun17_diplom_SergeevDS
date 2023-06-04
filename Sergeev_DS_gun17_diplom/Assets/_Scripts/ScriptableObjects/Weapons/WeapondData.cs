using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Metroidvania.Combat.Weapon
{
    [CreateAssetMenu(fileName = "New weapon data", menuName = "Data/Weapon data/Weapon")]
    public class WeapondData : ScriptableObject
    {
        public int AmountOfAttacks { get; protected set; }
        public float[] MovementSpeed { get; protected set; }
    }
}