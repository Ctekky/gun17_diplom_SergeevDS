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

        [Header("Wall Slide State")]
        public float wallSlideVelocity = 3f;

        [Header("Wall Climb State")]
        public float wallClimbVelocity = 3f;

        [Header("Check variables")]
        public float groundCheckRadius = 0.2f;
        public float wallCheckDistance = 0.5f;
        public LayerMask groundLayer;

        [Header("Wall Jump State")]
        public float wallJumpVelocity = 20f;
        public float wallJumpTime = 0.4f;
        public Vector2 wallJumpAngle = new Vector2(1, 2);

        [Header("Rope Climb State")]
        public float ropeClimbVelocity = 3f;
        public float ropeSwingVelocity = 3f;
        public float ropeSwingMultiplier = 0.5f;
        public float ropeTouchDelayforCollision = 0.5f;
    }
}


