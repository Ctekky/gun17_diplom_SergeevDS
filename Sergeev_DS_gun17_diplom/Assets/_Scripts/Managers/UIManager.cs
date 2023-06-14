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
        [Inject] private UICanvas _uiCanvasInventory;
        public event Action<ItemData, List<InventoryItem>> UICanvasCraftClicked;
        public void UpdateInventoryUI(List<InventoryItem> inventoryItems, ItemType itemType)
        {
            _uiCanvasInventory.UpdateCharacterListsUI(inventoryItems, itemType);
        }

        private void OnEnable()
        {
            _uiCanvasInventory.CraftClicked += (data, list) => UICanvasCraftClicked?.Invoke(data, list);
        }

        private void OnDisable()
        {
            _uiCanvasInventory.CraftClicked -= UICanvasCraftClicked;
        }
    }
    
}
