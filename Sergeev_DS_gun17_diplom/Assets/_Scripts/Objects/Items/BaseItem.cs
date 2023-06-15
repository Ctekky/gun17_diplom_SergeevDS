using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Metroidvania.Common.Items
{
    public class BaseItem : MonoBehaviour
    {
        [SerializeField] private ItemData itemData;
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private int minXVelocity;
        [SerializeField] private int maxXVelocity;
        [SerializeField] private int minYVelocity;
        [SerializeField] private int maxYVelocity;
        [SerializeField] private float timeBeforeInteract;
        private bool _canPickup;
        private float _currentTime;
        public event Action<BaseItem> OnPickuped;

        private void OnEnable()
        {
            rb.velocity = new Vector2(Random.Range(minXVelocity, maxXVelocity), Random.Range(minYVelocity, maxYVelocity));
            _currentTime = Time.time;
            _canPickup = false;
        }

        private void Update()
        {
            if(_canPickup) return;
            if (_currentTime + timeBeforeInteract < Time.time) _canPickup = true;
        }

        private void OnValidate()
        {
            if(itemData==null) return;
            GetComponent<SpriteRenderer>().sprite = itemData.icon;
            gameObject.name = "Item data " + itemData.name;
        }
        public ItemData GetItemData()
        {
            return itemData;
        }
        public void Pickup()
        {
            if(!_canPickup) return;
            OnPickuped?.Invoke(this);
            Destroy(gameObject);
        }
    }
}