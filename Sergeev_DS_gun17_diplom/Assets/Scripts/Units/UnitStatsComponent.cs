using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Metroidvania.Unit
{
    public class UnitStatsComponent : MonoBehaviour
    {
        [SerializeField, Range(1, 100)]
        private float _moveSpeed = 10f;
        [SerializeField, Range(1, 100)]
        private float _jumpForce = 10f;
        private SideType SideType;

        public float GetSetMoveSpeed { get => _moveSpeed; set { _moveSpeed = value; } }            
        public float GetSetJumpForce { get => _jumpForce; set { _jumpForce = value; } }
    }

}
