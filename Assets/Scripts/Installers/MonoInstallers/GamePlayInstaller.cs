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

public class GamePlayInstaller : MonoInstaller
{
    [SerializeField] private CinemachineVirtualCamera mainVirtualCamera;
    [SerializeField] private CinemachineVirtualCamera serveVirtualCamera;
    [SerializeField] private CinemachineVirtualCamera winVirtualCamera;

    [SerializeField] private PlayerInput gameplayPlayerInput;

    public override void InstallBindings()
    {
        Container.BindInterfacesTo<GamePlayController>().AsSingle();

        Container.BindInstance(mainVirtualCamera).WithId(GamePlayVirtualCameras.Main);
        Container.BindInstance(serveVirtualCamera).WithId(GamePlayVirtualCameras.Serve);
        Container.BindInstance(winVirtualCamera).WithId(GamePlayVirtualCameras.Win);

        Container.BindInstance(gameplayPlayerInput).AsSingle();
    }
}