using UnityEngine;

namespace  Metroidvania.Structs
{
    [System.Serializable]
    public struct SpawnersStruct
    {
        public Vector2 SpawnCoordinates;
        public GameObject EnemyPrefab;
        public int EnemyLevel;
    }
    
}
