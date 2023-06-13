using System;
using UnityEngine;
using Metroidvania.Interfaces;

namespace Metroidvania.Common.Items
{
    public class BaseItem : MonoBehaviour, IPickupable
    {
        [SerializeField] private ItemData itemData;
        public event Action<BaseItem> OnPickuped;

        private void OnValidate()
        {
            GetComponent<SpriteRenderer>().sprite = itemData.icon;
            gameObject.name = "Item data " + itemData.name;
        }
        public ItemData GetItemData()
        {
            return itemData;
        }
        public void Pickup()
        {
            OnPickuped?.Invoke(this);
            Destroy(gameObject);
        }
    }
}