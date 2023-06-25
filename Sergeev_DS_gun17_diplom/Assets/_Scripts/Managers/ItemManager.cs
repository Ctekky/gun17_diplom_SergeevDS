using System;
using System.Collections.Generic;
using System.Linq;
using Metroidvania.Common.Objects;
using Metroidvania.Common.Items;
using Metroidvania.Interfaces;
using Metroidvania.Player;
using UnityEditor;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Metroidvania.Managers
{
    public class ItemManager : MonoBehaviour, ISaveAndLoad
    {
        [Inject] private Player.Player _player;
        [SerializeField] private LootTableData lootTableData;
        private PlayerInventory _playerInventory;
        private readonly List<BaseItem> _itemsInGame = new List<BaseItem>();
        private List<BaseItem> _itemsToDrop = new List<BaseItem>();
        private List<BaseItem> _pickupables;
        private List<IInteractable> _interactableObjects;
        private List<Campfire> _campfires;
        private int _numberOfDropped;
        [Inject] private AudioManager _audioManager;
        public event Action GameSaved;

        [Header("Item database")] 
        private List<ItemData> _itemDataBase;
        public List<InventoryItem> loadedItems;


        private void Awake()
        {
            _interactableObjects = FindAllInteractableObjects();
            _campfires = FindAllCampfireOnMap();
            _pickupables = FindAllPickupable();
            foreach (var interactableObject in _interactableObjects)
            {
                interactableObject.Opened += ChooseItemToSpawn;
                interactableObject.Saved += SaveOnCampfire;
            }

            foreach (var pickupable in _pickupables)
            {
                pickupable.OnPickuped += AddItemToPlayer;
            }
        }

        private void SaveOnCampfire(Transform campfire)
        {
            if (campfire.GetComponent<Campfire>() != null)
            {
                DeactivateAllCampfires();
                campfire.GetComponent<Campfire>().SetState(true);
            }    
            GameSaved?.Invoke();
            _audioManager.PlaySFX((int)SFXSlots.CampfireBurningVariant2);
        }

        private void DeactivateAllCampfires()
        {
            foreach (var campfire in _campfires)
            {
                campfire.SetState(false);
            }
        }

        private void CreateItem(Vector2 coordinates, BaseItem itemToCreate)
        {
            var item = Instantiate(itemToCreate, coordinates, Quaternion.identity);
            var baseItem = item.GetComponent<BaseItem>();
            _itemsInGame.Add(baseItem);
            baseItem.OnPickuped += AddItemToPlayer;
        }

        public void ChooseItemToSpawn(LootType lootType, Vector2 coordinates)
        {
            foreach (var table in lootTableData.lootTableByType)
            {
                if (table.lootType != lootType) continue;
                _numberOfDropped = 0;
                foreach (var row in table.lootTable)
                {
                    if (Random.Range(0, 100) > row.dropChance) continue;
                    for (var i = 0; i < Random.Range(1, table.numberOfItemToDrop); i++)
                    {
                        CreateItem(coordinates, row.itemData);
                        _numberOfDropped++;
                        if (_numberOfDropped >= table.numberOfItemToDrop) break;
                    }

                    if (_numberOfDropped >= table.numberOfItemToDrop) break;
                }

                break;
            }
        }
        private void AddItemToPlayer(BaseItem item)
        {
            _itemsInGame.Remove(item);
            item.OnPickuped -= AddItemToPlayer;
            _audioManager.PlaySFX((int)SFXSlots.ItemPickup);
            if(_playerInventory == null) _playerInventory = _player.GetComponent<PlayerInventory>();
            _playerInventory.AddItem(item.GetItemData());
        }

        private void AddItemToPlayer(InventoryItem item)
        {
            _audioManager.PlaySFX((int)SFXSlots.ItemPickup);
            if(_playerInventory == null) _playerInventory = _player.GetComponent<PlayerInventory>();
            _playerInventory.AddItem(item);
        }

        private void Start()
        {
            if(_playerInventory == null) _playerInventory = _player.GetComponent<PlayerInventory>();
            _interactableObjects = FindAllInteractableObjects();

        }
        
        private void OnDisable()
        {
            foreach (var baseItem in _itemsInGame)
            {
                baseItem.OnPickuped -= AddItemToPlayer;
            }
            foreach (var interactableObject in _interactableObjects)
            {
                interactableObject.Opened -= ChooseItemToSpawn;
                interactableObject.Saved -= SaveOnCampfire;
            }
        }

        private List<ItemData> GetItemDataBase()
        {
            _itemDataBase = new List<ItemData>();
            var assetNames = AssetDatabase.FindAssets("", new[] { "Assets/_Scripts/Data/Items" });
            foreach (var soName in assetNames)
            {
                var soPath = AssetDatabase.GUIDToAssetPath(soName);
                var itemData = AssetDatabase.LoadAssetAtPath<ItemData>(soPath);
                _itemDataBase.Add(itemData);
            }

            return _itemDataBase;
        }

        public void LoadData(GameData.GameData gameData)
        {
            foreach (var pair in gameData.inventory)
            {
                foreach (var item in GetItemDataBase())
                {
                    if (item == null) continue;
                    if (item.itemID != pair.Key) continue;
                    var itemToLoad = new InventoryItem(item)
                    {
                        stackSize = pair.Value
                    };
                    AddItemToPlayer(itemToLoad);
                    loadedItems.Add(itemToLoad);
                }
            }
        }

        public void SaveData(ref GameData.GameData gameData)
        {
        }

        private List<IInteractable> FindAllInteractableObjects()
        {
            var interactableObjects = FindObjectsOfType<MonoBehaviour>().OfType<IInteractable>();
            return new List<IInteractable>(interactableObjects);
        }
        
        private List<Campfire> FindAllCampfireOnMap()
        {
            var campfires = FindObjectsOfType<MonoBehaviour>().OfType<Campfire>();
            return new List<Campfire>(campfires);
        }

        private List<BaseItem> FindAllPickupable()
        {
            var pickupable = FindObjectsOfType<MonoBehaviour>().OfType<BaseItem>();
            return new List<BaseItem>(pickupable);
        }
    }
}