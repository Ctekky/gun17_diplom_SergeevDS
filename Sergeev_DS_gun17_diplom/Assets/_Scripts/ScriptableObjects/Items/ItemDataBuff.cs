using UnityEngine;

namespace Metroidvania.Common.Items
{
    [CreateAssetMenu(fileName = "Base Item", menuName = "Data/Item data/Buff item")]
    public class ItemDataBuff : ItemData
    {
        public BuffType buffType;
        public int modifier;
    }
    
}
