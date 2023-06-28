using System;
using System.Collections.Generic;
using System.Linq;
using Metroidvania.Common.Items;
using Metroidvania.Enemy;
using Metroidvania.Structs;
using Unity.Mathematics;
using UnityEngine;
using Zenject;

namespace Metroidvania.Managers
{
    public class EnemyManager : MonoBehaviour
    {
        public event Action<Vector2, LootType> OnEnemyDied;
        private List<EnemySpawner> _spawnerScripts;
        private List<EnemySpawner> _spawnersList;
        private List<Transform> _spawners;
        [Inject] private AudioManager _audioManager;
        public event Action bossDied;

        private void Awake()
        {
            _spawners = new List<Transform>();
            _spawnersList = new List<EnemySpawner>();
            _spawnersList = FindAllSpawners();
        }

        private void Start()
        {
            foreach (var spawner in _spawnersList)
            {
                spawner.OnEnemyDied += EnemyDied;
                spawner.bossDied += OnBossDied;
                _spawners.Add(spawner.transform);
                spawner.audioManager = _audioManager;
            }
        }

        private void OnBossDied()
        {
            bossDied?.Invoke();
        }

        private void EnemyDied(Vector2 coordinates, LootType lootType)
        {
            OnEnemyDied?.Invoke(coordinates, lootType);
        }

        private void OnDisable()
        {
            foreach (var spawner in _spawnersList)
            {
                spawner.OnEnemyDied -= EnemyDied;
                spawner.bossDied -= OnBossDied;
            }
        }

        public void SpawnAll()
        {
            foreach (var spawner in _spawnersList)
            {
                spawner.SpawnEnemy();
            }
        }

        private List<EnemySpawner> FindAllSpawners()
        {
            var spawners = FindObjectsOfType<MonoBehaviour>().OfType<EnemySpawner>();
            return new List<EnemySpawner>(spawners);
        }
    }
}