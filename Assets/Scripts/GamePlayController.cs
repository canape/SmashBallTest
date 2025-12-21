using System;
using Zenject;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class GamePlayController : IInitializable, IDisposable, ITickable
{
    private readonly IPlayerManager playerManager;
    private readonly ICourtsManager courtManager;
    private readonly IHeroesManager heroesManager;

    private CinemachineVirtualCamera mainVirtualCamera;
    private CinemachineVirtualCamera serveVirtualCamera;
    private CinemachineVirtualCamera winVirtualCamera;

    private PlayerInput playerInput;
    private InputAction touchPressInputAction;
    
    private Court court;
    private Ball ball;
    private Hero hero;
    private Hero opponent;

    public GamePlayController(
        IPlayerManager playerManager, 
        ICourtsManager courtManager, 
        IHeroesManager heroesManager,
        [Inject(Id = GamePlayVirtualCameras.Main)] CinemachineVirtualCamera mainVirtualCamera,
        [Inject(Id = GamePlayVirtualCameras.Serve)] CinemachineVirtualCamera serveVirtualCamera,
        [Inject(Id = GamePlayVirtualCameras.Win)] CinemachineVirtualCamera winVirtualCamera,
        PlayerInput playerInput
        )
    {
        this.playerManager = playerManager;
        this.courtManager = courtManager;
        this.heroesManager = heroesManager;

        this.mainVirtualCamera = mainVirtualCamera;
        this.serveVirtualCamera = serveVirtualCamera;
        this.winVirtualCamera = winVirtualCamera;

        this.playerInput = playerInput;
    }

    public void Initialize()
    {
        var selectCourtId = playerManager.GetSelectedCourtId();
        var selectedHeroId = playerManager.GetSelectedHeroId();

        court = courtManager.GetCourtById(selectCourtId);

        ball = courtManager.GetBallByCourtId(selectCourtId);
        court.SetBall(ball);

        hero = heroesManager.GetHeroById(selectedHeroId);
        court.SetHero(hero);

        opponent = heroesManager.GetHeroById(0);
        court.SetOpponent(opponent);

        SetupCameras();
        SetupPlayerInput();

        float forceMagnitude = 5;
        var rb = ball.GetComponent<Rigidbody>();
        rb.AddForce(new Vector3(0, 0, 1) * forceMagnitude, ForceMode.Impulse);
    }

    public void Dispose()
    {
        touchPressInputAction.started -= TouchStarted;
        touchPressInputAction.canceled -= TouchEnded;
    }

    public void Tick()
    {
        //Debug.Log("Tick");
    }

    private void SetupCameras()
    {
        if (hero == null)
        {
            Debug.LogError("Trying to setup the cameras in the GamePlayController but there is not Hero in the scene");
            return;
        }

        mainVirtualCamera.Follow = hero.transform;
        serveVirtualCamera.Follow = hero.transform;
        winVirtualCamera.Follow = hero.transform;
        
        mainVirtualCamera.enabled = true;
        serveVirtualCamera.enabled = false;
        winVirtualCamera.enabled = false;
    }

    private void SetupPlayerInput()
    {
        touchPressInputAction = playerInput.actions["Touch"];

        touchPressInputAction.started += TouchStarted;
        touchPressInputAction.canceled += TouchEnded;
    }

    private void TouchStarted(InputAction.CallbackContext context)
    {
        var touchscreen = Touchscreen.current;
        if (touchscreen == null) return;
        var phase = touchscreen.primaryTouch.phase.ReadValue();
        Debug.Log($"Touch started in GamePlayController (phase={phase})");
    }

    private void TouchEnded(InputAction.CallbackContext context)
    {
        var touchscreen = Touchscreen.current;
        if (touchscreen == null) return;
        var phase = touchscreen.primaryTouch.phase.ReadValue();
        Debug.Log($"Touch ended in GamePlayController (phase={phase})");

        hero.Swing();
        
        //var jumpPosition = transform.position + new Vector3(0.0f, 1.0f, 0.0f);
        //transform.DOJump(transform.position, .2f, 1, 0.3f);
    }
}
