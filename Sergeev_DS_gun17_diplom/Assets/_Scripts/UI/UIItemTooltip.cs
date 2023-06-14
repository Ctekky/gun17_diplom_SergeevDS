using System;
using Metroidvania.Common.Items;
using TMPro;
using UnityEngine;

namespace  Metroidvania.UI
{
    public class UIItemTooltip : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI itemNameText;
        [SerializeField] private TextMeshProUGUI itemTypeText;
        [SerializeField] private TextMeshProUGUI itemDescription;

        [SerializeField] private Vector2 offset;

        private void Start()
        {
            gameObject.SetActive(false);
        }

        public void ShowTooltip<T>(T itemData, Vector2 position) where T : ItemData
        {
            var rt = GetComponent<RectTransform>();
            rt.position = position + offset;
            itemNameText.text = itemData.itemName;
            itemTypeText.text = itemData.itemType.ToString();
            itemDescription.text = itemData.itemDescription;
            gameObject.SetActive(true);
        }
        public void HideTooltip() => gameObject.SetActive(false);
    }
    
}
