using UnityEngine;
using Metroidvania.Interfaces;
using Metroidvania.Managers;
using Zenject;

namespace Metroidvania.Common.Items
{
    public class BaseItem : MonoBehaviour, IPickupable
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private ItemData itemData;
        
        private void Start()
        {
            
        }

        public void Pickup()
        {
        }
    }
}