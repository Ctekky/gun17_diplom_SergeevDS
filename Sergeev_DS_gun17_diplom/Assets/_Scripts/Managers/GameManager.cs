using System;
using UnityEngine;
using Zenject;

namespace Metroidvania.Managers
{
    public class GameManager : MonoBehaviour
    {
        [Inject] private EnemyManager _enemyManager;
        [Inject] private ItemManager _itemManager;

        private void Start()
        {
            _enemyManager.SpawnAll();
            _enemyManager.OnEnemyDied += EnemyDied;
        }

        private void EnemyDied(Vector2 coordinates)
        {
            Debug.Log($"Enemy died on coordinates {coordinates}");
        }

        private void OnDisable()
        {
            _enemyManager.OnEnemyDied -= EnemyDied;
        }
    }
}