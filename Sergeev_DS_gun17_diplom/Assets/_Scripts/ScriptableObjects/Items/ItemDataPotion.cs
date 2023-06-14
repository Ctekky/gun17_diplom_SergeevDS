using System.Collections.Generic;
using UnityEngine;

namespace Metroidvania.Common.Items
{
    [CreateAssetMenu(fileName = "Base Item", menuName = "Data/Item data/Potion item")]
    public class ItemDataPotion : ItemData
    {
        public PotionType potionType;
        public int potionModifier;
        public List<InventoryItem> craftingMaterials;
        public IEnumerable<ItemEffect> itemEffects;

        public void ItemEffect()
        {
            foreach (var effect in itemEffects)
            {
                effect.ExecuteEffect();
            }
        }
        
    }
    
}
