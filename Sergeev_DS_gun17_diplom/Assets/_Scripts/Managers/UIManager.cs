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
        public void UpdateInventoryUI(List<InventoryItem> inventoryItems, ItemType itemType)
        {
            _uiCanvas.UpdateCharacterListsUI(inventoryItems, itemType);
        }

        private void OnEnable()
        {
            _uiCanvas.CraftClicked += (data, list) => UICanvasCraftClicked?.Invoke(data, list);
        }

        private void OnDisable()
        {
            _uiCanvas.CraftClicked -= UICanvasCraftClicked;
        }
    }
    
}
