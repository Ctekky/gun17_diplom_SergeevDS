using System;
using System.Collections;
using System.Collections.Generic;
using Metroidvania.BaseUnit;
using Metroidvania.Common.Items;
using Metroidvania.Interfaces;
using Metroidvania.Player;
using Metroidvania.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Metroidvania.Managers
{
    public class GameManager : MonoBehaviour, ISaveAndLoad
    {
        [Inject] private EnemyManager _enemyManager;
        [Inject] private ItemManager _itemManager;
        [Inject] private UIManager _uiManager;
        [Inject] private Player.Player _player;
        [Inject] private SaveManager _saveManager;
        [Inject] private AudioManager _audioManager;
        private PlayerInputHandler _playerInputHandler;
        private string _currentScene;
        public float playingTime;
        [SerializeField] private string mainMenuScene = "MainMenu";
        [SerializeField] private string endMenuScene = "EndGame";
        [SerializeField, Range(1, 10)] private float delay = 1.5f;

        private void Start()
        {
            _enemyManager.SpawnAll();
            _audioManager.SetPlayer(_player);
            _player.InputHandler.SetGameplay();
        }

        private void Awake()
        {
            _playerInputHandler = _player.GetComponent<PlayerInputHandler>();
            _currentScene = SceneManager.GetActiveScene().name;
            _saveManager.sceneName = _currentScene;
        }

        private void OnEnable()
        {
            _uiManager.UICanvasCraftClicked += OnCraftClicked;
            _playerInputHandler.PressedOptionsUI += OnPressedOptionsUIUI;
            _playerInputHandler.PressedCharacterUI += OnPressedCharacterUI;
            _playerInputHandler.ClosedMenu += OnClosedMenu;
            _playerInputHandler.PressedCraftUI += OnPressedCraftUI;
            _player.GetComponent<PlayerInventory>().OnUpdateUI += InventoryUIUpdate;
            _player.Unit.GetUnitComponent<UnitStats>().OnHealthZero += PlayerDied;
            _enemyManager.OnEnemyDied += EnemyDied;
            _enemyManager.bossDied += OnBossDied;
            _itemManager.GameSaved += OnGameSave;
            _uiManager.GameSaved += OnGameSave;
            _uiManager.GameLoaded += OnGameLoad;
            _uiManager.GameEnded += OnGameEnd;
        }

        private void PlayerDied()
        {
            StartCoroutine(RespawnPlayer(delay, true));
        }

        private void OnGameEnd()
        {
            _playerInputHandler.SetMainMenu();
            StartCoroutine(LoadSceneWithFade(delay, mainMenuScene));
        }

        private void OnGameLoad()
        {
            StartCoroutine(RespawnPlayer(delay, false));
        }

        private void OnGameSave()
        {
            _saveManager.SaveGame();
        }

        private void OnClosedMenu()
        {
            if (_uiManager.UICanvas == null) return;
            _uiManager.UICanvas.CloseAllUI();
        }

        private void OnPressedOptionsUIUI()
        {
            if (_uiManager.UICanvas == null) return;
            _uiManager.UICanvas.SwitchToOptionsUI();
        }

        private void OnPressedCharacterUI()
        {
            if (_uiManager.UICanvas == null) return;
            _uiManager.UICanvas.SwitchToCharacterUI();
        }

        private void OnPressedCraftUI()
        {
            if (_uiManager.UICanvas == null) return;
            _uiManager.UICanvas.SwitchToCraftUI();
        }

        private void OnCraftClicked(ItemData itemData, List<InventoryItem> craftingMaterials)
        {
            if (_uiManager.UICanvas == null) return;
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
            _itemManager.GameSaved -= OnGameSave;
            _uiManager.GameSaved -= OnGameSave;
            _uiManager.GameLoaded -= OnGameLoad;
            _uiManager.GameEnded -= OnGameEnd;
            if (_player == null) return;
            _player.GetComponent<PlayerInventory>().OnUpdateUI -= InventoryUIUpdate;
            _playerInputHandler.PressedOptionsUI -= OnPressedOptionsUIUI;
            _playerInputHandler.PressedCharacterUI -= OnPressedCharacterUI;
            _playerInputHandler.ClosedMenu -= OnClosedMenu;
            _playerInputHandler.PressedCraftUI -= OnPressedCraftUI;
            _player.Unit.GetUnitComponent<UnitStats>().OnHealthZero -= PlayerDied;
            _enemyManager.bossDied -= OnBossDied;
        }

        private void OnBossDied()
        {
            _playerInputHandler.SetMainMenu();
            StartCoroutine(LoadSceneWithFade(delay, endMenuScene));
        }

        public void LoadData(GameData.GameData gameData)
        {
            if (gameData.lastScene != _currentScene) return;
            _currentScene = gameData.lastScene;
            playingTime = gameData.playingTime;
        }

        public void SaveData(ref GameData.GameData gameData)
        {
            gameData.lastScene = _currentScene;
            gameData.playingTime = playingTime + Time.time;
        }

        private IEnumerator LoadSceneWithFade(float delayScene, string scene)
        {
            if (_uiManager.UICanvas != null) _uiManager.UICanvas.FadeOut();
            yield return new WaitForSeconds(delayScene);
            SceneManager.LoadScene(scene);
        }

        private IEnumerator RespawnPlayer(float delayRespawn, bool isHealed)
        {
            if (_uiManager.UICanvas != null)
                _uiManager.UICanvas.SwitchToEndScreen();
            if (_uiManager.UICanvas != null)
                _uiManager.UICanvas.FadeIn();

            _player.SpawnPlayer(true);
            _saveManager.SaveGame();
            SceneManager.LoadScene(_currentScene);
            yield return new WaitForSeconds(delayRespawn);
        }
    }
}