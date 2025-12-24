using UnityEngine;
using Zenject;
using Cinemachine;
using UnityEngine.InputSystem;

public enum GamePlayVirtualCameras
{
    Main,
    Serve,
    Win
}

public enum PlayerType
{
    Hero,
    Opponent
}

public class LivesChangedSignal
{
    public PlayerType Player;
    public int Lives;
}

public class MatchFinishedSignal
{
    public PlayerType Winner;
}

public class GamePlayInstaller : MonoInstaller
{
    [SerializeField] private CinemachineVirtualCamera mainVirtualCamera;
    [SerializeField] private CinemachineVirtualCamera serveVirtualCamera;
    [SerializeField] private CinemachineVirtualCamera winVirtualCamera;

    [SerializeField] private PlayerInput gameplayPlayerInput;

    public override void InstallBindings()
    {
        InstallSignals();

        Container.Bind<IPlayerManager>().To<PlayerManager>().AsSingle();
        Container.Bind<ICourtsManager>().To<CourtsManager>().AsSingle();
        Container.Bind<IHeroesManager>().To<HeroesManager>().AsSingle();
        Container.Bind<IDialogsManager>().To<DialogsManager>().AsSingle();

        Container.BindInterfacesAndSelfTo<GamePlayController>().AsSingle();

        Container.BindInstance(mainVirtualCamera).WithId(GamePlayVirtualCameras.Main);
        Container.BindInstance(serveVirtualCamera).WithId(GamePlayVirtualCameras.Serve);
        Container.BindInstance(winVirtualCamera).WithId(GamePlayVirtualCameras.Win);

        Container.BindInstance(gameplayPlayerInput).AsSingle();

        InstallPresentersFactories();
    }
    
    private void InstallSignals()
    {
        SignalBusInstaller.Install(Container);
        Container.DeclareSignal<LivesChangedSignal>();
        Container.DeclareSignal<MatchFinishedSignal>();
    }

    private void InstallPresentersFactories()
    {
        Container.BindFactory<ScoreView, ScorePresenter, ScorePresenter.Factory>();
        Container.BindFactory<KickoffView, KickoffPresenter, KickoffPresenter.Factory>();
        Container.BindFactory<SmashView, SmashPresenter, SmashPresenter.Factory>();
        Container.BindFactory<WinView, WinPresenter, WinPresenter.Factory>();
    }
}