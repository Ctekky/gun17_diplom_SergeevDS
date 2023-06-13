using Metroidvania.Common.Items;

namespace Metroidvania.Structs
{
    [System.Serializable]
    public struct LootTable
    {
        public BaseItem itemData;
        public int dropChance;
    }
}