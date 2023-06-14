using System;
using System.Collections.Generic;
using Metroidvania.Common.Items;
using Metroidvania.Player;
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

        private void Start()
        {
            _enemyManager.SpawnAll();
            _enemyManager.OnEnemyDied += EnemyDied;
            _player.GetComponent<PlayerInventory>().OnUpdateUI += InventoryUIUpdate;
        }

        private void OnEnable()
        {
            _uiManager.UICanvasCraftClicked += OnCraftClicked;
        }

        private void OnCraftClicked(ItemData itemData, List<InventoryItem> craftingMaterials)
        {
            Debug.Log("Trying to craft some items");
            _player.GetComponent<PlayerInventory>().CanCraftItem(itemData, craftingMaterials);
        }

        private void EnemyDied(Vector2 coordinates, LootType lootType)
        {
            _itemManager.ChooseItemToSpawn(lootType, coordinates);
            Debug.Log($"Enemy died on coordinates {coordinates} and have {lootType}");
        }

        private void InventoryUIUpdate(List<InventoryItem> inventoryItems, ItemType itemType)
        {
            _uiManager.UpdateInventoryUI(inventoryItems, itemType);
        }

        private void OnDisable()
        {
            _enemyManager.OnEnemyDied -= EnemyDied;
            _player.GetComponent<PlayerInventory>().OnUpdateUI -= InventoryUIUpdate;
        }
    }
}