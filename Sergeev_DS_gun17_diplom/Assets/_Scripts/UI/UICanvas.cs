using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Metroidvania.BaseUnit;
using Metroidvania.Common.Items;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace  Metroidvania.UI
{
    public class UICanvas : MonoBehaviour
    {
        [Inject] private Player.Player _player;
        [Header("Inventory UnitUI")]
        private UIItemSlot[] _inventoryItemSlots;
        private UIItemSlot[] _buffItemSlots;
        private UIItemSlot[] _ammoItemSlots;
        private UIItemSlot[] _potionItemSlots;
        private UICraftSlot[] _craftItemSlots;
        private UIStatSlot[] _statSlots;
        private UIHealthSlot _healthSlot;
        private UICraftPanel _craftPanel;
        private UIOptionPanel _optionPanel;
        
        [SerializeField] private UIFadeScreen fadeScreen;
        [SerializeField] private GameObject diedText;
        [Space]
        
        [SerializeField] private Transform inventorySlotParent;
        [SerializeField] private Transform buffSlotParent;
        [SerializeField] private Transform ammoSlotParent;
        [SerializeField] private Transform potionSlotParent;
        [SerializeField] private Transform statSlotParent;
        [SerializeField] private UIItemTooltip itemTooltip;

        [SerializeField] private GameObject characterUI;
        [SerializeField] private GameObject craftUI;
        [SerializeField] private GameObject optionsUI;
        [SerializeField] private GameObject inGameUI;

        public event Action<ItemData, List<InventoryItem>> CraftClicked;
        public event Action GameSaved;
        public event Action GameLoaded;
        public event Action GameEnded; 

        private void Awake()
        {
            _inventoryItemSlots = inventorySlotParent.GetComponentsInChildren<UIItemSlot>();
            _buffItemSlots = buffSlotParent.GetComponentsInChildren<UIItemSlot>();
            _ammoItemSlots = ammoSlotParent.GetComponentsInChildren<UIItemSlot>();
            _potionItemSlots = potionSlotParent.GetComponentsInChildren<UIItemSlot>();
            _healthSlot = statSlotParent.GetComponentInChildren<UIHealthSlot>();
            _statSlots = statSlotParent.GetComponentsInChildren<UIStatSlot>();
            _craftPanel = GetComponentInChildren<UICraftPanel>();
            _optionPanel = GetComponentInChildren<UIOptionPanel>();
        }

        private void OnEnable()
        {
            ListenEventFromList(_inventoryItemSlots);
            ListenEventFromList(_buffItemSlots);
            ListenEventFromList(_ammoItemSlots);
            ListenEventFromList(_potionItemSlots);
            _craftPanel.CraftClicked += (data, list) => CraftClicked?.Invoke(data, list);
            _optionPanel.GameSavedFromUI += OnGameSaved;
            _optionPanel.GameLoadedFromUI += OnGameLoaded;
            _optionPanel.GameEndedFromUI += OnGameEnded;
        }

        private void Start()
        {
            UpdateStatsUI();
            UpdateHealthUI();
            CloseAllUI();
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

        private void OnGameSaved()
        {
            GameSaved?.Invoke();
        }

        private void OnGameLoaded()
        {
            GameLoaded?.Invoke();
        }

        private void OnGameEnded()
        {
            GameEnded?.Invoke();
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
            _craftPanel.CraftClicked -= CraftClicked;
            _optionPanel.GameSavedFromUI -= OnGameSaved;
            _optionPanel.GameLoadedFromUI -= OnGameLoaded;
            _optionPanel.GameEndedFromUI -= OnGameEnded;
        }
        public void SwitchTo(GameObject menu)
        {
            for (var i = 0; i < transform.childCount; i++)
            {
                var isFadeScreen = transform.GetChild(i).GetComponent<UIFadeScreen>() != null;
                if (!isFadeScreen)
                    transform.GetChild(i).gameObject.SetActive(false);
            }
            if(menu != null) menu.SetActive(true);
            UpdateStatsUI();
            UpdateHealthUI();
            inGameUI.gameObject.SetActive(true);
            
        }

        public void CloseAllUI()
        {
            for (var i = 0; i < transform.childCount; i++)
            {   
                var isFadeScreen = transform.GetChild(i).GetComponent<UIFadeScreen>() != null;
                if (!isFadeScreen) 
                    transform.GetChild(i).gameObject.SetActive(false);
            }
            inGameUI.gameObject.SetActive(true);
        }

        public void SwitchToCharacterUI()
        {
            SwitchTo(characterUI);
        }

        public void SwitchToCraftUI()
        {
            SwitchTo(craftUI);
        }

        public void SwitchToOptionsUI()
        {
            SwitchTo(optionsUI);
        }

        public void SwitchToEndScreen()
        {
            FadeOut();
        }

        public void DisableEndScreen()
        {
            diedText.SetActive(false);
        }
        private IEnumerator DieScreenCoroutine()
        {
            yield return new WaitForSeconds(1f);
            diedText.SetActive(true);
        }
        
        public void FadeOut() => fadeScreen.FadeOut();
        public void FadeIn() => fadeScreen.FadeIn();
    }
    
}
