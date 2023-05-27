using Zenject;
using UnityEngine;
using Metroidvania.Player;
using Metroidvania.Managers;

namespace Metroidvania.Common
{
    public class LocationInstaller : MonoInstaller
    {
        public Transform StartPoint;
        public GameObject PlayerPrefab;
        public override void InstallBindings()
        {
            Player.Player player = Container.InstantiatePrefabForComponent<Player.Player>(PlayerPrefab, StartPoint.position, Quaternion.identity, null);
            Container.Bind<Player.Player>().FromInstance(player).AsSingle();

        }
    }
}

