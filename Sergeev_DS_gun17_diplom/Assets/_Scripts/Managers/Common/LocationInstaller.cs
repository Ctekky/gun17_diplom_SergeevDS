using Metroidvania.Managers;
using Metroidvania.UI;
using Zenject;
using UnityEngine;

namespace Metroidvania.Common
{
    public class LocationInstaller : MonoInstaller
    {
        [SerializeField] private Transform startPoint;
        [SerializeField] private  GameObject playerPrefab;
        [SerializeField] private  ItemManager itemManagerPrefab;
        [SerializeField] private  EnemyManager enemyManagerPrefab;
        [SerializeField] private  GameManager gameManagerPrefab;
        [SerializeField] private  UIManager uiManagerPrefab;
        [SerializeField] private  UICanvas uiCanvasPrefab;
        [SerializeField] private  SaveManager saveManagerPrefab;
        [SerializeField] private  Transform managersParent;
        [SerializeField] private  Transform zeroPoint;

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
                Container.InstantiatePrefabForComponent<UIManager>(uiManagerPrefab, position, Quaternion.identity,
                    managersParent);
            Container.Bind<UIManager>().FromInstance(uiManager).AsSingle();
            var saveManager = Container.InstantiatePrefabForComponent<SaveManager>(saveManagerPrefab,
                position,
                Quaternion.identity, managersParent);
            Container.Bind<SaveManager>().FromInstance(saveManager).AsSingle();
            var gameManager =
                Container.InstantiatePrefabForComponent<GameManager>(gameManagerPrefab, position, Quaternion.identity,
                    managersParent);
            Container.Bind<GameManager>().FromInstance(gameManager).AsSingle();
            
        }
    }
}