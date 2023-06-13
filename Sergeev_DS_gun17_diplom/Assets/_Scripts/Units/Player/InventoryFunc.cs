using System.Collections.Generic;
using Metroidvania.Common.Items;

namespace Metroidvania.Player
{
    public class InventoryFunc
    {

        private void AddItemOrBuff(ICollection<InventoryItem> list, IDictionary<ItemDataBuff, InventoryItem> dictionary, ItemDataBuff item)
        {
            if (dictionary.TryGetValue(item, out var value))
            {
                value.AddStack();
            }
            else
            {
                var newItem = new InventoryItem(item);
                list.Add(newItem);
                dictionary.Add(item, newItem);
            }
        }
        private void RemoveItemOrBuff(ICollection<InventoryItem> list, IDictionary<ItemData, InventoryItem> dictionary,
            ItemData item)
        {
            if (!dictionary.TryGetValue(item, out var value)) return;
            if (value.stackSize <= 1)
            {
                list.Remove(value);
                dictionary.Remove(item);
            }
            else
            {
                value.RemoveStack();
            }
        }
        private void RemoveItemOrBuff(ICollection<InventoryItem> list, IDictionary<ItemDataBuff, InventoryItem> dictionary,
            ItemDataBuff item)
        {
            if (!dictionary.TryGetValue(item, out var value)) return;
            if (value.stackSize <= 1)
            {
                list.Remove(value);
                dictionary.Remove(item);
            }
            else
            {
                value.RemoveStack();
            }
        }
    }
}