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
        public void UpdateInventoryUI(List<InventoryItem> inventoryItems)
        {
            _uiCanvasInventory.UpdateSlotUI(inventoryItems);
        }
        public void UpdateBuffUI(List<InventoryItem> buffs)
        {
            _uiCanvasInventory.UpdateBuffUI(buffs);
        }
    }
    
}
