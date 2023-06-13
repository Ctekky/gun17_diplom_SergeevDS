using System;
using System.Collections.Generic;
using Metroidvania.Common.Items;
using Metroidvania.Player;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;
using Random = UnityEngine.Random;

namespace Metroidvania.Managers
{
    public class ItemManager : MonoBehaviour
    {
        [Inject] private Player.Player _player;
        [SerializeField] private LootTableData lootTableData; //for test
        private PlayerInventory _playerInventory;
        private readonly List<BaseItem> _itemsInGame = new List<BaseItem>();

        private void CreateItem(Vector2 coordinates, BaseItem itemToCreate)
        {
            var item = Instantiate(itemToCreate, coordinates, Quaternion.identity);
            var baseItem = item.GetComponent<BaseItem>();
            _itemsInGame.Add(baseItem);
            baseItem.OnPickuped += AddItemToPlayer;
        }

        public void SpawnItem(LootType lootType, Vector2 coordinates)
        {
            foreach (var table in lootTableData.lootTableByType)
            {
                if (table.lootType != lootType) continue;
                foreach (var row in table.lootTable)
                {
                     if(Random.Range(0, 100) > row.dropChance) continue;
                     CreateItem(coordinates, row.itemData);
                     break;
                }
                break;
            }
        }
        
        private void AddItemToPlayer(BaseItem item)
        {
            _itemsInGame.Remove(item);
            item.OnPickuped -= AddItemToPlayer;
            _playerInventory.AddItem(item.GetItemData());
        }
        private void Start()
        {
            _playerInventory = _player.GetComponent<PlayerInventory>();
        }

        private void OnDisable()
        {
            foreach (var baseItem in _itemsInGame)
            {
                baseItem.OnPickuped -= AddItemToPlayer;
            }
        }
    }
}

