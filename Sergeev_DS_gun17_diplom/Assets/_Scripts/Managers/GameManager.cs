using System;
using System.Collections.Generic;
using Metroidvania.Common.Items;
using Metroidvania.Player;
using Metroidvania.UI;
using UnityEngine;
using Zenject;

namespace Metroidvania.Managers
{
    public class GameManager : MonoBehaviour
    {
        [Inject] private EnemyManager _enemyManager;
        [Inject] private ItemManager _itemManager;
        [Inject] private UIManager _uiManager;
        [Inject] private Player.Player _player;
        private PlayerInputHandler _playerInputHandler;

        private void Start()
        {
            _enemyManager.SpawnAll();
            _enemyManager.OnEnemyDied += EnemyDied;
            _player.GetComponent<PlayerInventory>().OnUpdateUI += InventoryUIUpdate;
            
        }

        private void Awake()
        {
            _playerInputHandler = _player.GetComponent<PlayerInputHandler>();
        }

        private void OnEnable()
        {
            _uiManager.UICanvasCraftClicked += OnCraftClicked;
            _playerInputHandler.PressedOptionsUI += OnPressedOptionsUIUI;
            _playerInputHandler.PressedCharacterUI += OnPressedCharacterUI;
            _playerInputHandler.ClosedMenu += OnClosedMenu;
            _playerInputHandler.PressedCraftUI += OnPressedCraftUI;
        }

        private void OnClosedMenu()
        {
            _uiManager.UICanvas.CloseAllUI();
        }

        private void OnPressedOptionsUIUI()
        {
            _uiManager.UICanvas.SwitchToOptionsUI();
        }

        private void OnPressedCharacterUI()
        {
            _uiManager.UICanvas.SwitchToCharacterUI();
        }

        private void OnPressedCraftUI()
        {
            _uiManager.UICanvas.SwitchToCraftUI();
        }

        private void OnCraftClicked(ItemData itemData, List<InventoryItem> craftingMaterials)
        {
            var isCrafted = _player.GetComponent<PlayerInventory>().CanCraftItem(itemData, craftingMaterials);
            _uiManager.UICanvas.GetComponentInChildren<UICraftDetailPanel>().ShowErrorMessage(isCrafted);
        }

        private void EnemyDied(Vector2 coordinates, LootType lootType)
        {
            _itemManager.ChooseItemToSpawn(lootType, coordinates);
        }

        private void InventoryUIUpdate(List<InventoryItem> inventoryItems, ItemType itemType)
        {
            _uiManager.UpdateInventoryUI(inventoryItems, itemType);
        }

        private void OnDisable()
        {
            _enemyManager.OnEnemyDied -= EnemyDied;
            _player.GetComponent<PlayerInventory>().OnUpdateUI -= InventoryUIUpdate;
            _playerInputHandler.PressedOptionsUI -= OnPressedOptionsUIUI;
            _playerInputHandler.PressedCharacterUI -= OnPressedCharacterUI;
            _playerInputHandler.ClosedMenu -= OnClosedMenu;
            _playerInputHandler.PressedCraftUI -= OnPressedCraftUI;
        }
    }
}