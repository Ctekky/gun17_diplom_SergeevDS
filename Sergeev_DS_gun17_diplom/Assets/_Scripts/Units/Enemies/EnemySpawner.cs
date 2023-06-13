using System;
using Metroidvania.BaseUnit;
using UnityEngine;

namespace Metroidvania.Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        private GameObject _enemy;
        public event Action<Vector2, LootType> OnEnemyDied; 
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private int enemyLevel;

        public int EnemyLevel
        {
            get => enemyLevel;
            set => enemyLevel = value;
        }
        
        public void SpawnEnemy()
        {
            _enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            _enemy.GetComponentInChildren<UnitStats>().UnitLevel = enemyLevel;
            _enemy.GetComponentInChildren<EnemyDeathUnitComponent>().OnDied += EnemyDied;
        }

        private void EnemyDied(Vector2 coordinates, LootType lootType)
        {
            OnEnemyDied?.Invoke(coordinates, lootType);
        }

        private void OnDisable()
        {
            _enemy.GetComponentInChildren<EnemyDeathUnitComponent>().OnDied -= EnemyDied;
        }

        public void SetEnemyPrefab(GameObject prefab)
        {
            enemyPrefab = prefab;
        }
    }
}