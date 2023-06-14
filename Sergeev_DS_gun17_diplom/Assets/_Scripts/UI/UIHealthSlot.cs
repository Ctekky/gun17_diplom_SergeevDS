using UnityEngine;
using TMPro;

namespace Metroidvania.UI
{
    public class UIHealthSlot : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI statNameText;
        [SerializeField] private TextMeshProUGUI statValueText;
        private int _currentHealth;
        private int _maxHealth;

        public void UpdateHealthUI(int currentHealth, int maxHealth)
        {
            statNameText.text = "Health:";
            statValueText.text = $"{currentHealth.ToString()}/{maxHealth.ToString()}";
        }
    }
}
