using System;
using Zenject;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Text.RegularExpressions;
using Zenject.SpaceFighter;



#if UNITY_EDITOR
using UnityEditor;
#endif

public class GamePlayController : IInitializable, IDisposable, ITickable
{
    private readonly IPlayerManager playerManager;
    private readonly ICourtsManager courtManager;
    private readonly IHeroesManager heroesManager;

    private CinemachineVirtualCamera mainVirtualCamera;
    private CinemachineVirtualCamera serveVirtualCamera;
    private CinemachineVirtualCamera winVirtualCamera;

    private SignalBus signalBus;
    private PlayerInput playerInput;
    private InputAction touchPressInputAction;
    
    private Court court;
    private Ball ball;
    private Hero hero;
    private Hero opponent;

    public GamePlayController(
        SignalBus signalBus,
        IPlayerManager playerManager, 
        ICourtsManager courtManager, 
        IHeroesManager heroesManager,
        [Inject(Id = GamePlayVirtualCameras.Main)] CinemachineVirtualCamera mainVirtualCamera,
        [Inject(Id = GamePlayVirtualCameras.Serve)] CinemachineVirtualCamera serveVirtualCamera,
        [Inject(Id = GamePlayVirtualCameras.Win)] CinemachineVirtualCamera winVirtualCamera,
        PlayerInput playerInput
        )
    {
        this.signalBus = signalBus;
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

    }

    public void Dispose()
    {
        touchPressInputAction.started -= TouchStarted;
        touchPressInputAction.canceled -= TouchEnded;
    }

    public void Tick()
    {
        OpponentAI();
        DetectHits();
    }

    private void OpponentAI()
    {
        float distance = Vector3.Distance(opponent.transform.position, ball.transform.position);

        if (distance <= 2)
        {
            opponent.Swing();

            Vector3 hitNormal = (ball.transform.position - opponent.transform.position).normalized;
            float forceMagnitude = 5;
            ball.SetDirectionAndForce(hitNormal, forceMagnitude);
        }
    }

    private void DetectHits()
    {
        DetectOpponentHit();
        DetectHeroHit();
    }

    private void DetectOpponentHit()
    {
        float distance = Vector3.Distance(opponent.transform.position, ball.transform.position);
        if (distance <= 1)
        {
            opponent.SubstractLive();
            signalBus.Fire(new LivesChangedSignal() { Player = PlayerType.Opponent, Lives = opponent.Lives });
            
            if (opponent.Lives <= 0)
            {
                //signalBus.Fire(new MatchFinishedSignal() { Winner = PlayerType.Hero });
            }
        }
    }

    private void DetectHeroHit()
    {
        float distance = Vector3.Distance(hero.transform.position, ball.transform.position);
        if (distance <= 1)
        {
            hero.SubstractLive();
            signalBus.Fire(new LivesChangedSignal() { Player = PlayerType.Hero, Lives = hero.Lives });

            if (hero.Lives <= 0)
            {
                //signalBus.Fire(new MatchFinishedSignal() { Winner = PlayerType.Opponent });
            }
        }
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
        
        float distance = Vector3.Distance(hero.transform.position, ball.transform.position);
        if (distance <= 2)
        {
            Vector3 hitNormal = (ball.transform.position - hero.transform.position).normalized;
            float forceMagnitude = 5;
            ball.SetDirectionAndForce(hitNormal, forceMagnitude);
        }
    }
}
