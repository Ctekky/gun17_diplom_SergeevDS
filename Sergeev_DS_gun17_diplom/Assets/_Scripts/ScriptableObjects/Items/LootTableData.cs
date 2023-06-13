using Metroidvania.Structs;
using UnityEngine;

namespace Metroidvania.Common.Items
{
    [CreateAssetMenu(fileName = "LootTableData", menuName = "Data/Loot table data/Loot table")]
    public class LootTableData : ScriptableObject
    {
        public LootTableByType[] lootTableByType;
    }
    
}
