using System;
using System.Collections.Generic;
using System.Linq;
using Metroidvania.BaseUnit;
using Metroidvania.Common.Items;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace  Metroidvania.UI
{
    public class UICanvas : MonoBehaviour
    {
        [Inject] private Player.Player _player;
        [Header("Inventory UI")]
        private UIItemSlot[] _inventoryItemSlots;
        private UIItemSlot[] _buffItemSlots;
        private UIItemSlot[] _ammoItemSlots;
        private UIItemSlot[] _potionItemSlots;
        private UICraftSlot[] _craftItemSlots;
        private UIStatSlot[] _statSlots;
        private UIHealthSlot _healthSlot;
        
        [SerializeField] private Transform inventorySlotParent;
        [SerializeField] private Transform buffSlotParent;
        [SerializeField] private Transform ammoSlotParent;
        [SerializeField] private Transform potionSlotParent;
        [SerializeField] private Transform craftSlotParent;
        [SerializeField] private Transform statSlotParent;
        [SerializeField] private UIItemTooltip itemTooltip;

        public event Action<ItemData, List<InventoryItem>> CraftClicked; 
        private void Start()
        {
            _inventoryItemSlots = inventorySlotParent.GetComponentsInChildren<UIItemSlot>();
            ListenEventFromList(_inventoryItemSlots);
            _buffItemSlots = buffSlotParent.GetComponentsInChildren<UIItemSlot>();
            ListenEventFromList(_buffItemSlots);
            _ammoItemSlots = ammoSlotParent.GetComponentsInChildren<UIItemSlot>();
            ListenEventFromList(_ammoItemSlots);
            _potionItemSlots = potionSlotParent.GetComponentsInChildren<UIItemSlot>();
            ListenEventFromList(_potionItemSlots);
            _craftItemSlots = craftSlotParent.GetComponentsInChildren<UICraftSlot>();
            _healthSlot = statSlotParent.GetComponentInChildren<UIHealthSlot>();
            _statSlots = statSlotParent.GetComponentsInChildren<UIStatSlot>();

            foreach (var craftSlot in _craftItemSlots)
            {
                craftSlot.UISlotCraftClicked += (data, list) => CraftClicked?.Invoke(data, list);
            }
            UpdateStatsUI();
            UpdateHealthUI();
        }

        private void ListenEventFromList(IEnumerable<UIItemSlot> slots)
        {
            foreach (var slot in slots)
            {
                slot.PointerEnter += ShowTooltip;
                slot.PointerExit += HideTooltip;
            }
        }
        private void UnlistenEventFromList(IEnumerable<UIItemSlot> slots)
        {
            foreach (var slot in slots)
            {
                slot.PointerEnter -= ShowTooltip;
                slot.PointerExit -= HideTooltip;
            }
        }

        private void ShowTooltip(ItemData itemData, Vector2 position)
        {
            itemTooltip.ShowTooltip(itemData, position);
        }
        private void HideTooltip() => itemTooltip.HideTooltip();
        private void UpdateStatsUI()
        {
            foreach (var stat in _statSlots)
            {
                var playerStats = _player.Unit.GetUnitComponent<UnitStats>();
                stat.UpdateStat(playerStats.GetStat(stat.StatType));
            }
        }
        private void UpdateHealthUI()
        {
            var playerStats = _player.Unit.GetUnitComponent<UnitStats>();
            _healthSlot.UpdateHealthUI(playerStats.GetCurrentHealth(),playerStats.GetMaxHealthValue());
        }
        public void UpdateCharacterListsUI(List<InventoryItem> items, ItemType itemType)
        {
            switch (itemType)
            {
                case ItemType.Material:
                    UpdateSlotUI(items, _inventoryItemSlots);
                    break;
                case ItemType.Buff:
                    UpdateSlotUI(items, _buffItemSlots);
                    break;
                case ItemType.Ammo:
                    UpdateSlotUI(items, _ammoItemSlots);
                    break;
                case ItemType.Potion:
                    UpdateSlotUI(items, _potionItemSlots);
                    break;
                default:
                    break;
            }
        }
        private void UpdateSlotUI(IReadOnlyList<InventoryItem> inventoryItems, IEnumerable<UIItemSlot> uiItemSlots)
        {
            var itemSlots = uiItemSlots as UIItemSlot[] ?? uiItemSlots.ToArray();
            foreach (var slot in itemSlots)
            {
                slot.CleanUpSlot();
            }
            for (var i = 0; i < inventoryItems.Count; i++)
            {
                itemSlots[i].UpdateSlot(inventoryItems[i]);
            }
        }
        private void OnDisable()
        {
            UnlistenEventFromList(_inventoryItemSlots);
            UnlistenEventFromList(_buffItemSlots);
            UnlistenEventFromList(_ammoItemSlots);
            UnlistenEventFromList(_potionItemSlots);
            foreach (var craftSlot in _craftItemSlots)
            {
                craftSlot.UISlotCraftClicked -= CraftClicked;
            }
        }
        public void SwitchTo(GameObject menu)
        {
            for (var i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
            if(menu != null) menu.SetActive(true);
            UpdateStatsUI();
            UpdateHealthUI();
        }
    }
    
}
