using Metroidvania.Managers;
using Metroidvania.Player;
using Metroidvania.UI;
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
        public UIManager uiManagerPrefab;
        public UICanvas uiCanvasPrefab;

        public override void InstallBindings()
        {
            var position1 = startPoint.position;
            var position = zeroPoint.position;
            var player =
                Container.InstantiatePrefabForComponent<Player.Player>(playerPrefab, position1,
                    Quaternion.identity, null);
            Container.Bind<Player.Player>().FromInstance(player).AsSingle();
            var uiCanvas =
                Container.InstantiatePrefabForComponent<UICanvas>(uiCanvasPrefab, position, Quaternion.identity, null);
            Container.Bind<UICanvas>().FromInstance(uiCanvas).AsSingle();
            var itemManager = Container.InstantiatePrefabForComponent<ItemManager>(itemManagerPrefab,
                position,
                Quaternion.identity, managersParent);
            Container.Bind<ItemManager>().FromInstance(itemManager).AsSingle();
            var enemyManager = Container.InstantiatePrefabForComponent<EnemyManager>(enemyManagerPrefab,
                position, Quaternion.identity, managersParent);
            Container.Bind<EnemyManager>().FromInstance(enemyManager).AsSingle();
            var uiManager =
                Container.InstantiatePrefabForComponent<UIManager>(uiManagerPrefab, position, Quaternion.identity, managersParent);
            Container.Bind<UIManager>().FromInstance(uiManager).AsSingle();
            var gameManager =
                Container.InstantiatePrefabForComponent<GameManager>(gameManagerPrefab, position, Quaternion.identity,
                    managersParent);
            Container.Bind<GameManager>().FromInstance(gameManager).AsSingle();

        }
    }
}