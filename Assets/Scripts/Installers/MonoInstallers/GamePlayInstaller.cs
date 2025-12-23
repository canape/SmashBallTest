using UnityEngine;
using Zenject;
using Cinemachine;
using UnityEngine.InputSystem;
using Zenject.SpaceFighter;

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

        Container.BindInterfacesTo<GamePlayController>().AsSingle();

        Container.BindInstance(mainVirtualCamera).WithId(GamePlayVirtualCameras.Main);
        Container.BindInstance(serveVirtualCamera).WithId(GamePlayVirtualCameras.Serve);
        Container.BindInstance(winVirtualCamera).WithId(GamePlayVirtualCameras.Win);

        Container.BindInstance(gameplayPlayerInput).AsSingle();

        Container.BindFactory<ScoreView, ScoreViewPresenter, ScoreViewPresenter.Factory>();
    }

    private void InstallSignals()
    {
        SignalBusInstaller.Install(Container);
        Container.DeclareSignal<LivesChangedSignal>();
        Container.DeclareSignal<MatchFinishedSignal>();
    }
}