using Metroidvania.BaseUnit;
using UnityEngine;
namespace Metroidvania.Structs
{
    [System.Serializable]
    public struct WeaponAttackDetails
    {
        public string attackName;
        public float movementSpeed;
        public Stats damageAmount;
        public float knockbackStrength;
        public Vector2 knockbackAngle;
    }

}


