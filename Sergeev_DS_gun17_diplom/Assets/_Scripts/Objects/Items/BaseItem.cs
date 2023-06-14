using System;
using UnityEngine;
using Metroidvania.Interfaces;
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
        public event Action<BaseItem> OnPickuped;

        private void OnEnable()
        {
            rb.velocity = new Vector2(Random.Range(minXVelocity, maxXVelocity), Random.Range(minYVelocity, maxYVelocity));
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
            OnPickuped?.Invoke(this);
            Destroy(gameObject);
        }
    }
}