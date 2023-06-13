using System;
using Metroidvania.Common.Items;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

namespace Metroidvania.UI
{
    public class UIItemSlot : MonoBehaviour
    {
        [SerializeField] private Image itemImage;
        [SerializeField] private TextMeshProUGUI itemText;

        public InventoryItem item;

        public void UpdateSlot(InventoryItem newItem)
        {
            item = newItem;
            itemImage.color = Color.white;
            if (item == null) return;
            itemImage.sprite = item.ItemData.icon;
            itemText.text = item.stackSize > 1 ? item.stackSize.ToString() : "";
        }

        public void CleanUpSlot()
        {
            item = null;
            itemImage.color = Color.clear;
            itemImage.sprite = null;
            itemText.text = "";
        }
    }
}