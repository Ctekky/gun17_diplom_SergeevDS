using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Metroidvania.Player
{
    [CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/Player Data/Base Data")]
    public class PlayerData : ScriptableObject
    {
        [Header("Move State")]
        public float movementVelocity = 10f;

        [Header("Jump State")]
        public float jumpVelocity = 15f;
        public int jumpCount = 1;

        [Header("In Air State")]
        public float lastMomentJumpTime = 0.2f;
        public float jumpHeighMultiplier = 0.5f;

        [Header("Check variables")]
        public float groundCheckRadius;
        public LayerMask groundLayer;
    }
}


