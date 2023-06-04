using System;
using Metroidvania.BaseUnit;
using UnityEngine;

namespace Metroidvania.Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        private GameObject _enemy;
        public event Action<Vector2> OnEnemyDied; 
        [SerializeField] private GameObject enemyPrefab;
        
        public void SpawnEnemy()
        {
            _enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            _enemy.GetComponentInChildren<EnemyDeathUnitComponent>().OnDied += EnemyDied;
        }

        private void EnemyDied(Vector2 coordinates)
        {
            OnEnemyDied?.Invoke(coordinates);
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