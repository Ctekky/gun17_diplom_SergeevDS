using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Metroidvania.Common.Items
{
    [CreateAssetMenu(fileName = "Base Item", menuName = "Data/Item data/Potion item")]
    public class ItemDataPotion : ItemData
    {
        public PotionType potionType;
        public int potionModifier;
    }
    
}
