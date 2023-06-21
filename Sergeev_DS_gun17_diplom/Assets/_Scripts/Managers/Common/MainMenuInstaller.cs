
using Metroidvania.Managers;
using Metroidvania.Player;
using Metroidvania.UI;
using Zenject;
using UnityEngine;


namespace Metroidvania.Common
{
    public class MainMenuInstaller : MonoInstaller
    {
        [SerializeField] private  Transform zeroPoint;
        [SerializeField] private  Transform managersParent;
        [SerializeField] private  SaveManager saveManagerPrefab;

        public override void InstallBindings()
        {
            var position = zeroPoint.position;
            var saveManager = Container.InstantiatePrefabForComponent<SaveManager>(saveManagerPrefab,
                position,
                Quaternion.identity, managersParent);
            Container.Bind<SaveManager>().FromInstance(saveManager).AsSingle();

        }
    }
}