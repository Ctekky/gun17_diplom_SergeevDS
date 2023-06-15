using System;
using System.Collections.Generic;
using System.Linq;
using Metroidvania.Common.Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Metroidvania.UI
{
    public class UICraftDetailPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI itemName;
        [SerializeField] private TextMeshProUGUI itemDescription;
        [SerializeField] private Transform materialListParent;
        [SerializeField] private GameObject materialSlotPrefab;
        [SerializeField] private GameObject errorMessage;
        [SerializeField] private List<GameObject> materialSlots;
        
        private ItemData _itemToCraft;
        private List<InventoryItem> _requiredMaterials;
        public event Action<ItemData, List<InventoryItem>> CraftButtonClicked; 

        private void ClearList()
        {
            foreach (var materialSlot in materialSlots.ToList())
            {
                materialSlots.Remove(materialSlot);
                Destroy(materialSlot);
            }
        }

        public void SetupPanel(ItemData item, List<InventoryItem> requireMaterials)
        {
            errorMessage.SetActive(false);
            _itemToCraft = item;
            _requiredMaterials = requireMaterials;
            itemName.text = _itemToCraft.itemName;
            itemDescription.text = _itemToCraft.itemDescription;
            ClearList();
            materialSlots = new List<GameObject>();
            foreach (var craftMaterial in _requiredMaterials)
            {
                var newCraftSlot = Instantiate(materialSlotPrefab, materialListParent);
                newCraftSlot.GetComponent<UIMaterialSlot>().SetupSlot<ItemData>(craftMaterial.ItemData, craftMaterial.stackSize);
                materialSlots.Add(newCraftSlot);
            }
        }
        public void OnCraftButtonClick()
        {
            CraftButtonClicked?.Invoke(_itemToCraft, _requiredMaterials);
        }

        public void ShowErrorMessage(bool isCrafted)
        {
            if(isCrafted) return;
            errorMessage.SetActive(true);
        }
    }
    
}
