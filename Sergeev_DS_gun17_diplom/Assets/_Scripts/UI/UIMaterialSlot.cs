using Metroidvania.Common.Items;

namespace Metroidvania.UI
{
    public class UIMaterialSlot : UIItemSlot
    {
        public void SetupSlot<T>(ItemData itemData, int count) where T : ItemData
        {
            if(itemData == null) return;
            item.ItemData = itemData;
            itemImage.sprite = itemData.icon;
            itemText.text = itemData.itemName + " x" + count;
        }
    }
    
}
