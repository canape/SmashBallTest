using Zenject;

public class ProjectInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<IPlayerManager>().To<PlayerManager>().AsSingle();
        Container.Bind<ICourtsManager>().To<CourtsManager>().AsSingle();
        Container.Bind<IHeroesManager>().To<HeroesManager>().AsSingle();
    }
}