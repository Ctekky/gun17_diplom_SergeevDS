using Metroidvania.Player;
using UnityEngine.EventSystems;

namespace Metroidvania.UI
{
    public class UIPotionSlot : UIItemSlot
    {
        private PlayerInventory _playerInventory;
        protected override void Awake()
        {
            _playerInventory = Player.GetComponent<PlayerInventory>();
        }
        
        public override void OnPointerDown(PointerEventData eventData)
        {
            _playerInventory.ChoosePotionSlot(item);
        }
    }
    
}
