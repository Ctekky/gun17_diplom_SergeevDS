using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Metroidvania.Enemy
{
    [CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/Enemy Data/Base Data")]
    public class EnemyData : ScriptableObject
    {
        [Header("Move State")]
        public float movementVelocity = 3f;

        [Header("Check Variables")]
        public float wallCheckDistance = 0.5f;
        public float ledgeCheckDistance = 0.2f;
        public LayerMask groundLayer;

        [Header("Idle State")]
        public float minIdleTime = 1f;
        public float maxIdleTime = 4f;

        [Header("Detected State")]
        public float minAggroDistance = 3f;
        public float maxAggroDistance = 4f;
        public float actionTime = 1.5f;
        public LayerMask playerLayer;

        [Header("Enemy Charge State")]
        public float chargeVelocity = 6f;
        public float chargeTime = 2f;

        [Header("Enemy Look for player State")]
        public int amountOfTurns = 2;
        public float timeOfTurns = 0.5f;

        [Header("Enemy Attack State")]
        public float closeRangeActionDistance = 1f;
        public float longRangeActionDistance = 3f;
        public float meleeAttackRadius = 0.5f;
        public float meleeAttackDamage = 10f;

        [Header("Enemy stats")]
        public float health = 30f;
        public float damageHopSpeed = 3f;

        [Header("Enemy dead state")]
        public GameObject deathParticles;

    }
}

