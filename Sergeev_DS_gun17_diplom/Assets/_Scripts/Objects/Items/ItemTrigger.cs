using Metroidvania.Interfaces;
using UnityEngine;

namespace Metroidvania.Common.Items
{
    public class ItemTrigger : MonoBehaviour, IPickupable
    {
        [SerializeField] private BaseItem baseItem;
        public void Pickup()
        {
            baseItem.Pickup();
        }
    }
    
}
