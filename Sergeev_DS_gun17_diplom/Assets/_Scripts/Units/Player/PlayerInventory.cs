using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Metroidvania.Combat.Weapon;
using Metroidvania.Common.Items;
using Metroidvania.Interfaces;
using Metroidvania.UI;

namespace Metroidvania.Player
{
    public class PlayerInventory : MonoBehaviour, ISaveAndLoad
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
        [SerializeField] private InventoryItem currentAmmo;
        [SerializeField] private InventoryItem potion1;
        [SerializeField] private InventoryItem potion2;
        [SerializeField] private InventoryItem potion3;
        [SerializeField] private InventoryItem potion4;
        [SerializeField] private List<InventoryItem> potionOnHotBar;
        private InventoryItem _lastUsedAmmo;

        public event Action<List<InventoryItem>, ItemType> OnUpdateUI;
        public event Action<BuffType, int> OnAppliedBuff;
        public event Action<GameObject> SetNewAmmo;
        public event Action<InventoryItem> SetNewAmmoIcon;
        public event Action SetEmptyAmmo;
        public event Action<InventoryItem, PotionSlotNumber> SetNewPotionIcon;
        public event Action<PotionSlotNumber> SetEmptyPotion;
        public event Action<int> HealthPotionUsed;

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

        public void SetCurrentAmmo(InventoryItem item)
        {
            GetItemFromList(item, _ammo, out currentAmmo);
            var currentAmmoData = currentAmmo.ItemData as ItemDataAmmo;
            if (currentAmmoData != null) SetNewAmmo?.Invoke(currentAmmoData.arrowPrefab);
            SetNewAmmoIcon?.Invoke(item);
        }
        private void SetCurrentPotion(InventoryItem item, PotionSlotNumber slotNumber)
        {
            switch (slotNumber)
            {
                case PotionSlotNumber.First:
                    GetItemFromList(item, _potion, out potion1);
                    SetNewPotionIcon?.Invoke(item, slotNumber);
                    break;
                case PotionSlotNumber.Second:
                    GetItemFromList(item, _potion, out potion2);
                    SetNewPotionIcon?.Invoke(item, slotNumber);
                    break;
                case PotionSlotNumber.Third:
                    GetItemFromList(item, _potion, out potion3);
                    SetNewPotionIcon?.Invoke(item, slotNumber);
                    break;
                case PotionSlotNumber.Fourth:
                    GetItemFromList(item, _potion, out potion4);
                    SetNewPotionIcon?.Invoke(item, slotNumber);
                    break;
                default:
                    break;
            }
        }
        private void GetItemFromList(InventoryItem item, List<InventoryItem> list, out InventoryItem value)
        {
            var index = list.IndexOf(item);
            value = list[index];
        }
        public void ChoosePotionSlot(InventoryItem item)
        {
            if(potionOnHotBar.Contains(item)) return;
            switch (potionOnHotBar.Count)
            {
                case 0:
                    SetCurrentPotion(item, PotionSlotNumber.First);
                    potionOnHotBar.Add(item);
                    break;
                case 1:
                    SetCurrentPotion(item, PotionSlotNumber.Second);
                    potionOnHotBar.Add(item);
                    break;
                case 2:
                    SetCurrentPotion(item, PotionSlotNumber.Third);
                    potionOnHotBar.Add(item);
                    break;
                case 3:
                    SetCurrentPotion(item, PotionSlotNumber.Fourth);
                    potionOnHotBar.Add(item);
                    break;
                case 4:
                    Debug.Log("no slots");
                    break;
                default:
                    break;
            }
        }
        private void DecreasePotionInSlot(InventoryItem slot, PotionSlotNumber number)
        {
            if(slot.ItemData == null) return;
            if (slot.stackSize == 1)
            {
                RemoveItem(slot.ItemData);
                potionOnHotBar.Remove(slot);
                SetEmptyPotion?.Invoke(number);
                switch (number)
                {
                    case PotionSlotNumber.First:
                        potion1 = null;
                        break;
                    case PotionSlotNumber.Second:
                        potion2 = null;
                        break;
                    case PotionSlotNumber.Third:
                        potion3 = null;
                        break;
                    case PotionSlotNumber.Fourth:
                        potion4 = null;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                RemoveItem(slot.ItemData);
                SetNewPotionIcon?.Invoke(slot, number);
            }
        }
        public void DecreasePotion(PotionSlotNumber number)
        {
            switch (number)
            {
                case PotionSlotNumber.First:
                    DecreasePotionInSlot(potion1, PotionSlotNumber.First);
                    break;
                case PotionSlotNumber.Second:
                    DecreasePotionInSlot(potion2, PotionSlotNumber.Second);
                    break;
                case PotionSlotNumber.Third:
                    DecreasePotionInSlot(potion3, PotionSlotNumber.Third);
                    break;
                case PotionSlotNumber.Fourth:
                    DecreasePotionInSlot(potion4, PotionSlotNumber.Fourth);
                    break;
                default:
                    break;
            }
        }
        public void UsePotionInSlot(PotionSlotNumber number)
        {
            switch (number)
            {
                case PotionSlotNumber.First:
                    UsePotion(potion1);
                   break;
                case PotionSlotNumber.Second:
                    UsePotion(potion2);
                    break;
                case PotionSlotNumber.Third:
                    UsePotion(potion3);
                    break;
                case PotionSlotNumber.Fourth:
                    UsePotion(potion4);
                    break;
                default:
                    break;
            }
        }

        private void UsePotion(InventoryItem potion)
        {
            if (potion.ItemData == null) return;
            var potionData = potion.ItemData as ItemDataPotion;
            if (potionData == null) return;
            switch (potionData.potionType)
            {
                case PotionType.Health:
                    HealthPotionUsed?.Invoke(potionData.potionModifier);
                    break;
                case PotionType.Buff:
                    //TODO buff potions
                    break;
                default:
                    break;
            }
        }

        public void DecreaseAmmo()
        {
            if (currentAmmo.stackSize == 1)
            {
                RemoveItem(currentAmmo.ItemData);
                currentAmmo = null;
                _lastUsedAmmo = null;
                SetNextAmmo();
            }
            else
            {
                RemoveItem(currentAmmo.ItemData);
                SetNewAmmoIcon?.Invoke(currentAmmo);
            }
        }
        private void UpdateAmmoSlot(Dictionary<ItemDataAmmo, InventoryItem> dictionary, ItemDataAmmo ammo)
        {
            if (!dictionary.TryGetValue(ammo, out var value)) return;
            if (currentAmmo == null) return;
            var index = _ammo.IndexOf(value);
            if (currentAmmo == _ammo[index]) SetNewAmmoIcon?.Invoke(value);
        }

        private void UpdatePotionSlot(Dictionary<ItemDataPotion, InventoryItem> dictionary, ItemDataPotion potion,
            PotionSlotNumber number, InventoryItem potionSlot)
        {
            if (!dictionary.TryGetValue(potion, out var value)) return;
            if (potionSlot == null) return;
            var index = _potion.IndexOf(value);
            if (potionSlot == _potion[index]) SetNewPotionIcon?.Invoke(value, number);
        }

    public void SwitchAmmo()
        {
            _lastUsedAmmo = currentAmmo;
            currentAmmo = null;
            SetNextAmmo();
        }
        
        private void SetNextAmmo()
        {
            if(currentAmmo != null)
                if(currentAmmo.ItemData != null) return;
            foreach (var ammo in _ammo.Where(ammo => ammo != _lastUsedAmmo))
            {
                SetCurrentAmmo(ammo);
                break;
            }
            if(currentAmmo == null) SetEmptyAmmo?.Invoke();
        }

        public bool CanShootArrow()
        {
            if (currentAmmo == null)
            {
                _lastUsedAmmo = null;
                SetNextAmmo();
                if (currentAmmo == null) return false;
                return currentAmmo.stackSize > 0;
            }
            else
            {
                if (currentAmmo.ItemData == null)
                {
                    SetNextAmmo();
                    if (currentAmmo == null) return false;
                    return currentAmmo.stackSize > 0;
                }
                return currentAmmo.stackSize > 0;
            }
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
                    UpdateAmmoSlot(_ammoDictionary, ammo);
                    break;
                case ItemType.Potion:
                    var potion = item as ItemDataPotion;
                    AddItemToListAndDictionary(_potion, _potionDictionary, potion);
                    UpdateAllPotionSlots(potion);
                    OnUpdateUI?.Invoke(_potion, ItemType.Potion);
                    break;
                default:
                    break;
            }
        }

        public void AddItem(InventoryItem item)
        {
            for (var i = 0; i < item.stackSize; i++)
            {
                AddItem(item.ItemData);
            }
            Debug.Log("Item Loaded");
        }
        private void RemoveItem(ItemData item)
        {
            if(item == null) return;
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
                    UpdateAmmoSlot(_ammoDictionary, ammo);
                    break;
                case ItemType.Potion:
                    var potion = item as ItemDataPotion;
                    RemoveItemFromListAndDictionary(_potion, _potionDictionary, potion);
                    UpdateAllPotionSlots(potion);
                    OnUpdateUI?.Invoke(_potion, ItemType.Potion);
                    break;
                default:
                    break;
            }
        }
        
        private void UpdateAllPotionSlots(ItemDataPotion potion)
        {
            UpdatePotionSlot(_potionDictionary, potion, PotionSlotNumber.First, potion1);
            UpdatePotionSlot(_potionDictionary, potion, PotionSlotNumber.Second, potion2);
            UpdatePotionSlot(_potionDictionary, potion, PotionSlotNumber.Third, potion3);
            UpdatePotionSlot(_potionDictionary, potion, PotionSlotNumber.Fourth, potion4);
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
            return true;
        }

        public void LoadData(GameData.GameData gameData)
        {
        }

        public void SaveData(ref GameData.GameData gameData)
        {
            gameData.inventory.Clear();

            foreach (var pair in _inventoryDictionary)
            {
                gameData.inventory.Add(pair.Key.itemID, pair.Value.stackSize);
            }
            foreach (var pair in _ammoDictionary)
            {
                gameData.inventory.Add(pair.Key.itemID, pair.Value.stackSize);
            }
            foreach (var pair in _potionDictionary)
            {
                gameData.inventory.Add(pair.Key.itemID, pair.Value.stackSize);
            }
            foreach (var pair in _buffDictionary)
            {
                gameData.inventory.Add(pair.Key.itemID, pair.Value.stackSize);
            }
        }
    }
}