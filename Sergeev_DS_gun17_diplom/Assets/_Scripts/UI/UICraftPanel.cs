using System;
using System.Collections.Generic;
using Metroidvania.Common.Items;
using UnityEngine;

namespace Metroidvania.UI
{
    public class UICraftPanel : MonoBehaviour
    {
        [SerializeField] private Transform craftPanelParent;
        [SerializeField] private GameObject craftSlotPrefab;
        [SerializeField] private List<ItemData> craftList;
        [SerializeField] private List<UICraftSlot> craftSlots;
        [SerializeField] private UICraftDetailPanel craftDetailPanel;
        private List<UICraftSlot> _craftSlots;
        
        public event Action<ItemData, List<InventoryItem>> CraftClicked; 

        private void Awake()
        {
            craftSlots = new List<UICraftSlot>();
            SetupCraftList();
            craftDetailPanel.gameObject.SetActive(false);
        }
        private void AssignCraftEvent()
        {
            foreach (var craftSlot in craftSlots)
            {
                craftSlot.UISlotCraftClicked += CraftSlotClicked;
            }
        }

        private void CraftSlotClicked(ItemData itemData, List<InventoryItem> requireMaterials)
        {
            craftDetailPanel.SetupPanel(itemData, requireMaterials);
            craftDetailPanel.gameObject.SetActive(true);
            craftDetailPanel.CraftButtonClicked += (data, list) => CraftClicked?.Invoke(data, list);
        }
        private void SetupCraftList()
        {
            foreach (var craftSlot in craftSlots)
            {
                Destroy(craftSlot.gameObject);
            }
            
            foreach (var craftItem in craftList)
            {
                var newCraftSlot = Instantiate(craftSlotPrefab, craftPanelParent);
                var craftSlotScript = newCraftSlot.GetComponent<UICraftSlot>();
                craftSlotScript.SetupSlot<ItemData>(craftItem);
                craftSlots.Add(craftSlotScript);
            }
        }

        private void OnEnable()
        {
            AssignCraftEvent();
        }

        private void OnDisable()
        {
            foreach (var craftSlot in craftSlots)
            {
                craftSlot.UISlotCraftClicked -= CraftSlotClicked;
            }
            craftDetailPanel.gameObject.SetActive(false);
        }
    }
}
