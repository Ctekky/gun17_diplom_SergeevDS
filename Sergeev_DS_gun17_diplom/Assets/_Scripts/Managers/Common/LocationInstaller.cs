using Metroidvania.Managers;
using Zenject;
using UnityEngine;


namespace Metroidvania.Common
{
    public class LocationInstaller : MonoInstaller
    {
        public Transform startPoint;
        public Transform zeroPoint;
        public Transform managersParent;
        public GameObject playerPrefab;
        public ItemManager itemManagerPrefab;
        public EnemyManager enemyManagerPrefab;
        public GameManager gameManagerPrefab;

        public override void InstallBindings()
        {
            var player =
                Container.InstantiatePrefabForComponent<Player.Player>(playerPrefab, startPoint.position,
                    Quaternion.identity, null);
            Container.Bind<Player.Player>().FromInstance(player).AsSingle();
            var position = zeroPoint.position;
            var itemManager = Container.InstantiatePrefabForComponent<ItemManager>(itemManagerPrefab,
                position,
                Quaternion.identity, managersParent);
            Container.Bind<ItemManager>().FromInstance(itemManager).AsSingle();
            var enemyManager = Container.InstantiatePrefabForComponent<EnemyManager>(enemyManagerPrefab,
                position, Quaternion.identity, managersParent);
            Container.Bind<EnemyManager>().FromInstance(enemyManager).AsSingle();
            var gameManager =
                Container.InstantiatePrefabForComponent<GameManager>(gameManagerPrefab, position, Quaternion.identity,
                    managersParent);
            Container.Bind<GameManager>().FromInstance(gameManager).AsSingle();
        }
    }
}