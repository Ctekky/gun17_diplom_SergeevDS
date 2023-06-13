using System.Collections.Generic;
using Metroidvania.Common.Items;
using UnityEngine;

namespace  Metroidvania.UI
{
    public class UICanvas : MonoBehaviour
    {
        [Header("Inventory UI")]
        private UIItemSlot[] _inventoryItemSlots;
        private UIItemSlot[] _buffItemSlots;
        
        [SerializeField] private Transform inventorySlotParent;
        [SerializeField] private Transform buffSlotParent;
        private void Start()
        {
            _inventoryItemSlots = inventorySlotParent.GetComponentsInChildren<UIItemSlot>();
            _buffItemSlots = buffSlotParent.GetComponentsInChildren<UIItemSlot>();
        }
        public void UpdateSlotUI(List<InventoryItem> inventoryItems)
        {
            foreach (var slot in _inventoryItemSlots)
            {
                slot.CleanUpSlot();
            }
            for (var i = 0; i < inventoryItems.Count; i++)
            {
                _inventoryItemSlots[i].UpdateSlot(inventoryItems[i]);
            }
        }
        public void UpdateBuffUI(List<InventoryItem> buffList)
        {
            foreach (var slot in _buffItemSlots)
            {
                slot.CleanUpSlot();
            }
            for (var i = 0; i < buffList.Count; i++)
            {
                _buffItemSlots[i].UpdateSlot(buffList[i]);
            }
        }
    }
    
}
