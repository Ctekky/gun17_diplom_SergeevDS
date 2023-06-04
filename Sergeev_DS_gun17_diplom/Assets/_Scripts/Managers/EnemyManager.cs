using System;
using System.Collections.Generic;
using System.Linq;
using Metroidvania.Enemy;
using Metroidvania.Structs;
using Unity.Mathematics;
using UnityEngine;

namespace Metroidvania.Managers
{
    public class EnemyManager : MonoBehaviour
    {
        private List<Transform> _spawners;
        [SerializeField] private List<SpawnersStruct> _spawnersStructs;
        [SerializeField] private GameObject spawnerPrefab;
        public event Action<Vector2> OnEnemyDied;
        private List<EnemySpawner> _spawnerScripts;

        private void Awake()
        {
            _spawners = new List<Transform>();
            _spawnerScripts = new List<EnemySpawner>();
            foreach (var spawnerData in _spawnersStructs)
            {
                var spawner = Instantiate(spawnerPrefab, spawnerData.SpawnCoordinates, quaternion.identity);
                var spawnerScript = spawner.GetComponent<EnemySpawner>();
                spawnerScript.SetEnemyPrefab(spawnerData.EnemyPrefab); 
                spawnerScript.OnEnemyDied += EnemyDied;
                _spawners.Add(spawner.transform);
                _spawnerScripts.Add(spawnerScript);
            }
        }
        private void EnemyDied(Vector2 coordinates)
        {
            OnEnemyDied?.Invoke(coordinates);
        }

        private void OnDisable()
        {
            foreach (var spawnerScript in _spawnerScripts)
            {
                spawnerScript.OnEnemyDied -= OnEnemyDied;
            }
        }

        public void SpawnAll()
        {
            foreach (var spawner in _spawners)
            {
                var spawnerScript = spawner.GetComponent<EnemySpawner>();
                spawnerScript.SpawnEnemy();
            }
        }

    }
}