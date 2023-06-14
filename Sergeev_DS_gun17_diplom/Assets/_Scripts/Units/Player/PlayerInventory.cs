using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Metroidvania.Combat.Weapon;
using Metroidvania.Common.Items;

namespace Metroidvania.Player
{
    public class PlayerInventory : MonoBehaviour
    {
        public Weapon[] weapons;
        [SerializeField] private List<InventoryItem> _inventory;
        [SerializeField] private Dictionary<ItemData, InventoryItem> _inventoryDictionary;
        [SerializeField] private List<InventoryItem> _buff;
        [SerializeField] private Dictionary<ItemDataBuff, InventoryItem> _buffDictionary;
        [SerializeField] private List<InventoryItem> _ammo;
        [SerializeField] private Dictionary<ItemDataAmmo, InventoryItem> _ammoDictionary;
        [SerializeField] private List<InventoryItem> _potion;
        [SerializeField] private Dictionary<ItemDataPotion, InventoryItem> _potionDictionary;
        public event Action<List<InventoryItem>> OnUpdateInventoryUI;
        public event Action<List<InventoryItem>, ItemType> OnUpdateUI; 
        public event Action<List<InventoryItem>> OnUpdateBuffUI;
        
        public event Action<List<InventoryItem>> OnUpdateAmmoUI;
        public event Action<List<InventoryItem>> OnUpdatePotionUI;
        public event Action<BuffType, int> OnAppliedBuff;

        private void Start()
        {
            _inventory = new List<InventoryItem>();
            _inventoryDictionary = new Dictionary<ItemData, InventoryItem>();
            _buff = new List<InventoryItem>();
            _buffDictionary = new Dictionary<ItemDataBuff, InventoryItem>();
            _ammo = new List<InventoryItem>();
            _ammoDictionary = new Dictionary<ItemDataAmmo, InventoryItem>();
            _potion = new List<InventoryItem>();
            _potionDictionary = new Dictionary<ItemDataPotion, InventoryItem>();
        }
        public void AddItem(ItemData item)
        {
            switch (item.itemType)
            {
                case ItemType.Material:
                    AddItemToListAndDictionary(_inventory, _inventoryDictionary, item);
                    OnUpdateUI?.Invoke(_inventory, ItemType.Material);
                    break;
                case ItemType.Buff:
                    var buff = item as ItemDataBuff;
                    AddItemToListAndDictionary(_buff, _buffDictionary, buff);
                    OnUpdateUI?.Invoke(_buff, ItemType.Buff);
                    if (buff != null) OnAppliedBuff?.Invoke(buff.buffType, buff.modifier);
                    break;
                case ItemType.Ammo:
                    var ammo = item as ItemDataAmmo;
                    AddItemToListAndDictionary(_ammo, _ammoDictionary, ammo);
                    OnUpdateUI?.Invoke(_ammo, ItemType.Ammo);
                    break;
                case ItemType.Potion:
                    var potion = item as ItemDataPotion;
                    AddItemToListAndDictionary(_potion, _potionDictionary, potion);
                    OnUpdateUI?.Invoke(_potion, ItemType.Potion);
                    break;
                default:
                    break;
            }
        }
        public void RemoveItem(ItemData item)
        {
            switch (item.itemType)
            {
                case ItemType.Material:
                    RemoveItemFromListAndDictionary(_inventory, _inventoryDictionary, item);
                    OnUpdateUI?.Invoke(_inventory, ItemType.Material);
                    break;
                case ItemType.Buff:
                    var buff = item as ItemDataBuff;
                    RemoveItemFromListAndDictionary(_buff, _buffDictionary, buff);
                    OnUpdateUI?.Invoke(_buff, ItemType.Buff);
                    break;
                case ItemType.Ammo:
                    var ammo = item as ItemDataAmmo;
                    RemoveItemFromListAndDictionary(_ammo, _ammoDictionary, ammo);
                    OnUpdateUI?.Invoke(_ammo, ItemType.Ammo);
                    break;
                case ItemType.Potion:
                    var potion = item as ItemDataPotion;
                    RemoveItemFromListAndDictionary(_potion, _potionDictionary, potion);
                    OnUpdateUI?.Invoke(_potion, ItemType.Potion);
                    break;
                default:
                    break;
            }
        }
        private void AddItemToListAndDictionary<T>(ICollection<InventoryItem> list, IDictionary<T, InventoryItem> dictionary,
            T item) where T: ItemData
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
        private void RemoveItemFromListAndDictionary<T>(ICollection<InventoryItem> list, IDictionary<T, InventoryItem> dictionary,
            T item) where T: ItemData
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

        public bool CanCraftItem<T>(T itemToCraft, IEnumerable<InventoryItem> requiredMaterials) where T : ItemData
        {
            var materialsToRemove = new List<InventoryItem>();
            foreach (var item in requiredMaterials.ToList())
            {
                if (_inventoryDictionary.TryGetValue(item.ItemData, out var inventoryValue))
                {
                    if (inventoryValue.stackSize < item.stackSize)
                    {
                        Debug.Log("Not enough materials");
                        return false;
                    }
                    materialsToRemove.Add(inventoryValue);
                }
                else
                {
                    Debug.Log("Not enough materials");
                    return false;  
                }
            }

            foreach (var itemToRemove in materialsToRemove.ToList())
            {
                RemoveItem(itemToRemove.ItemData);
            }
            AddItem(itemToCraft);
            Debug.Log($"Craft item {itemToCraft}");
            return true;
        }
    }
}