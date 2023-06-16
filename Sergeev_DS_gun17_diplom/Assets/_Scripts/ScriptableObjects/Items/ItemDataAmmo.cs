using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Metroidvania.Common.Items
{
    [CreateAssetMenu(fileName = "Base Item", menuName = "Data/Item data/Ammo item")]
    public class ItemDataAmmo : ItemData
    {
        public ArrowType arrowType;
        public GameObject arrowPrefab;
        public List<InventoryItem> craftingMaterials;
        
    }
}