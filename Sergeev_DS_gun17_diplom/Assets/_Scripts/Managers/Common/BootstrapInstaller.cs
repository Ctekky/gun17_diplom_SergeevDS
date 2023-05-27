using Zenject;
using Metroidvania.Generic;
using Metroidvania.Managers;


namespace Metroidvania.Common
{
    public class BootstrapInstaller : MonoInstaller
    {

        public PlayerManager PlayerManagerPrefab;
        public SkillManager SkillManagerPrefab;
        public override void InstallBindings()
        {
            Container.Bind<PlayerManager>().To<PlayerManager>().FromComponentInNewPrefab(PlayerManagerPrefab).AsSingle();
            Container.Bind<SkillManager>().To<SkillManager>().FromComponentInNewPrefab(SkillManagerPrefab).AsSingle();
        }
    }
}

