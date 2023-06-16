using System;
using Metroidvania.Common.Items;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine;
using Zenject;

namespace Metroidvania.UI
{
    public class UIItemSlot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] protected Image itemImage;
        [SerializeField] protected TextMeshProUGUI itemText;
        [Inject] protected Player.Player Player;

        public InventoryItem item;
        public event Action<ItemData, Vector2> PointerEnter;
        public event Action PointerExit; 

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

        public virtual void OnPointerDown(PointerEventData eventData)
        {
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            if(item == null) return;
            if(item.ItemData == null) return;
            PointerEnter?.Invoke(item.ItemData, transform.position);
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            if(item == null) return;
            if(item.ItemData == null) return;
            PointerExit?.Invoke();
        }

        protected virtual void Awake()
        {
        }
    }
}