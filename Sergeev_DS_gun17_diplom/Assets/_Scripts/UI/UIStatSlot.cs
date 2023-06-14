using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Metroidvania.UI 
{
    public class UIStatSlot : MonoBehaviour
    {
        [SerializeField] private StatType statName;
        [SerializeField] private TextMeshProUGUI statNameText;
        [SerializeField] private TextMeshProUGUI statValueText;
        public StatType StatType
        {
            get => statName;
            set => statName = value;
        }

        private void OnValidate()
        {
            gameObject.name = "Stat " + statName;
            if (statNameText != null) statNameText.text = statName.ToString();
        }

        public void UpdateStat(int statValue)
        {
            statNameText.text = statName.ToString();
            statValueText.text = $"{statValue}";
        }
    }
}


