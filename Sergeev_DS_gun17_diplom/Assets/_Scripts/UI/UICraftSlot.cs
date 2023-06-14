using System;
using System.Collections.Generic;
using Metroidvania.Common.Items;
using UnityEngine.EventSystems;

namespace Metroidvania.UI
{
    public class UICraftSlot : UIItemSlot
    {
        public event Action<ItemData, List<InventoryItem>> UISlotCraftClicked;
        public override void OnPointerDown(PointerEventData eventData)
        {
            switch (item.ItemData.itemType)
            {
                case ItemType.Material:
                    break;
                case ItemType.Buff:
                    break;
                case ItemType.Ammo:
                    var craftAmmo = item.ItemData as ItemDataAmmo;
                    UISlotCraftClicked?.Invoke(craftAmmo, craftAmmo.craftingMaterials);
                    break;
                case ItemType.Potion:
                    var craftPotion = item.ItemData as ItemDataPotion;
                    UISlotCraftClicked?.Invoke(craftPotion, craftPotion.craftingMaterials);
                    break;
                default:
                    break;
            }
        }

        private void OnEnable()
        {
            UpdateSlot(item);
        }
    }
}