using System;
using TMPro;
using UnityEngine;

namespace Metroidvania.Common.Objects
{
    public class PopUpText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private string textToShow;
        private bool _hideText;
        public bool GetSetHideText
        {
            get => _hideText;
            set => _hideText = value;
        }

        private void Start()
        {
            _hideText = false;
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (other.GetComponentInParent<Player.Player>() == null) return;
            if(_hideText) return;
            text.text = textToShow;
        }
        public void OnTriggerExit2D(Collider2D other)
        {
            if (other.GetComponentInParent<Player.Player>() == null) return;
            text.text = "";
        }
    }
    
}
