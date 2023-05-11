using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Metroidvania.Unit
{
    public class ColliderComponent : MonoBehaviour
    {
        private Collider _collider;
        [SerializeField]
        private int _id = 0;

        public int GetID => _id;

        public bool Enable
        {
            get => _collider.enabled;
            set => _collider.enabled = value;
        }
        void Start()
        {
            if (_collider == null) _collider = GetComponent<Collider>();
            _collider.enabled = false;
        }
    }
}