using Zenject;
using Metroidvania.Generic;
using Metroidvania.Managers;
using UnityEngine.Serialization;


namespace Metroidvania.Common
{
    public class BootstrapInstaller : MonoInstaller
    {

        //[FormerlySerializedAs("PlayerManagerPrefab")] public PlayerManager playerManagerPrefab;
        //[FormerlySerializedAs("SkillManagerPrefab")] public SkillManager skillManagerPrefab;
        public override void InstallBindings()
        {
            //Container.Bind<PlayerManager>().To<PlayerManager>().FromComponentInNewPrefab(playerManagerPrefab).AsSingle();
            //Container.Bind<SkillManager>().To<SkillManager>().FromComponentInNewPrefab(skillManagerPrefab).AsSingle();
        }
    }
}

