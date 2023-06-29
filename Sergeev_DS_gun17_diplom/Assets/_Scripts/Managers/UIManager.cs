using System;
using System.Collections.Generic;
using Metroidvania.Common.Items;
using Metroidvania.UI;
using UnityEngine;
using Zenject;

namespace Metroidvania.Managers
{
    public class UIManager : MonoBehaviour
    {
        [Inject] private UICanvas _uiCanvas;

        public UICanvas UICanvas => _uiCanvas;

        public event Action<ItemData, List<InventoryItem>> UICanvasCraftClicked;
        public event Action GameSaved;
        public event Action GameLoaded;
        public event Action GameEnded;

        public void UpdateInventoryUI(List<InventoryItem> inventoryItems, ItemType itemType)
        {
            _uiCanvas.UpdateCharacterListsUI(inventoryItems, itemType);
        }

        private void OnEnable()
        {
            _uiCanvas.CraftClicked += (data, list) => UICanvasCraftClicked?.Invoke(data, list);
            _uiCanvas.GameSaved += OnGameSaved;
            _uiCanvas.GameLoaded += OnGameLoaded;
            _uiCanvas.GameEnded += OnGameEnded;
        }

        private void OnGameEnded()
        {
            GameEnded?.Invoke();
        }

        private void OnGameLoaded()
        {
            GameLoaded?.Invoke();
        }

        private void OnGameSaved()
        {
            GameSaved?.Invoke();
        }

        private void OnDisable()
        {
            _uiCanvas.CraftClicked -= UICanvasCraftClicked;
        }
    }
}