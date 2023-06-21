using Zenject;
using Metroidvania.Generic;
using Metroidvania.Managers;
using Metroidvania.UI;
using UnityEngine;

namespace Metroidvania.Common
{
    public class BootstrapInstaller : MonoInstaller
    {

        public  ItemManager itemManagerPrefab;
        public  EnemyManager enemyManagerPrefab;
        public  GameManager gameManagerPrefab;
        public  UIManager uiManagerPrefab;
        public  UICanvas uiCanvasPrefab;
        public  SaveManager saveManagerPrefab;
        public  Transform managersParent;
        public  Transform zeroPoint;
        public override void InstallBindings()
        {
            //Container.Bind<PlayerManager>().To<PlayerManager>().FromComponentInNewPrefab(playerManagerPrefab).AsSingle();
            //Container.Bind<SkillManager>().To<SkillManager>().FromComponentInNewPrefab(skillManagerPrefab).AsSingle();
            var position = zeroPoint.position;
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

