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
            _player.GetComponent<PlayerInventory>().OnUpdateInventoryUI += InventoryUIUpdate;
            _player.GetComponent<PlayerInventory>().OnUpdateBuffUI += BuffUIUpdate;
        }

        private void EnemyDied(Vector2 coordinates, LootType lootType)
        {
            _itemManager.SpawnItem(lootType, coordinates);
            Debug.Log($"Enemy died on coordinates {coordinates} and have {lootType}");
        }

        private void InventoryUIUpdate(List<InventoryItem> inventoryItems)
        {
            _uiManager.UpdateInventoryUI(inventoryItems);
        }
        private void BuffUIUpdate(List<InventoryItem> buffs)
        {
            _uiManager.UpdateBuffUI(buffs);
        }

        private void OnDisable()
        {
            _enemyManager.OnEnemyDied -= EnemyDied;
            _player.GetComponent<PlayerInventory>().OnUpdateInventoryUI -= InventoryUIUpdate;
            _player.GetComponent<PlayerInventory>().OnUpdateInventoryUI -= BuffUIUpdate;
        }
    }
}