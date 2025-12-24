using System;
using Zenject;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using Terresquall;


#if UNITY_EDITOR
using UnityEditor;
#endif

public enum GameplayStatus
{
    Undefined,
    Point,
    Smash,
    Serve,
    End
}

public class GamePlayController : IInitializable, IDisposable, ITickable
{
    private readonly IPlayerManager playerManager;
    private readonly ICourtsManager courtManager;
    private readonly IHeroesManager heroesManager;

    private IDialogsManager dialogsManager;

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

    private bool touchActivated;
    private bool heroMovementActivated;

    private GameplayStatus status;

    public GamePlayController(
        SignalBus signalBus,
        IPlayerManager playerManager, 
        ICourtsManager courtManager, 
        IHeroesManager heroesManager,
        IDialogsManager dialogsManager,
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

        this.dialogsManager = dialogsManager;

        this.mainVirtualCamera = mainVirtualCamera;
        this.serveVirtualCamera = serveVirtualCamera;
        this.winVirtualCamera = winVirtualCamera;

        this.playerInput = playerInput;

        status = GameplayStatus.Undefined;
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

        touchPressInputAction = playerInput.actions["Touch"];

        SetupCameras();
        StartMatch();
    }

    public void Dispose()
    {
        DeactivateTouch();
        DeactivatePlayerMovement();
    }

    public void Tick()
    {
        if (status != GameplayStatus.Point)
        {
            return;
        }

        AwesomeOpponentAI();
        DetectHits();
        MoveHero();
    }

    private void AwesomeOpponentAI()
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

            if (opponent.Lives <= 0)
            {
                FinishMatch(opponent);
            }
            else
            {
                FinishPoint(opponent);
            }
        }
    }

    private void DetectHeroHit()
    {
        float distance = Vector3.Distance(hero.transform.position, ball.transform.position);
        if (distance <= 1)
        {
            hero.SubstractLive();

            if (hero.Lives <= 0)
            {
                FinishMatch(hero);
            }
            else
            {
                FinishPoint(hero);
            }
        }
    }

    private void MoveHero()
    {
        if (!heroMovementActivated)
        {
            return;
        }

        float moveX = VirtualJoystick.GetAxis("Horizontal");
        float moveZ = VirtualJoystick.GetAxis("Vertical");

        var direction = new Vector3(moveX, 0, moveZ).normalized;
        hero.Move(direction, 5);
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

    private void ActivateTouch()
    {
        if (touchActivated)
        {
            return;
        }

        touchActivated = true;
        touchPressInputAction.canceled += TouchEnded;
    }

    private void DeactivateTouch()
    {
        if (!touchActivated)
        {
            return;
        }
                
        touchActivated = false;
        touchPressInputAction.canceled -= TouchEnded;
    }

    private void ActivatePlayerMovement()
    {
        heroMovementActivated = true;
    }

    private void DeactivatePlayerMovement()
    {
        heroMovementActivated = false;
    }

    private void TouchEnded(InputAction.CallbackContext context)
    {
        var touchscreen = Touchscreen.current;
        if (touchscreen == null)
        {
            return;
        }

        hero.Swing();
        
        float distance = Vector3.Distance(hero.transform.position, ball.transform.position);
        if (distance <= 3)
        {
            if (status == GameplayStatus.Serve)
            {
                status = GameplayStatus.Point;
                ActivatePlayerMovement();
            }

            Vector3 hitNormal = (ball.transform.position - hero.transform.position).normalized;
            float forceMagnitude = 5;
            ball.SetDirectionAndForce(hitNormal, forceMagnitude);
        }
    }

    public void Serve()
    {
        status = GameplayStatus.Serve;
        dialogsManager.CreateDialog(DialogType.Kickoff);
        court.ResetPositions();
        ball.PauseMovement();

        ActivateTouch();
        DeactivatePlayerMovement();
    }

    public void FinishPoint(Hero loser)
    {
        status = GameplayStatus.Smash;
        dialogsManager.CreateDialog(DialogType.Smash);
        ball.PauseMovement();
        signalBus.Fire(new LivesChangedSignal() { Player = loser.Role, Lives = loser.Lives });

        DeactivateTouch();
        DeactivatePlayerMovement();
    }

    public void StartMatch()
    {
        // Important: signalBus.Fire should be inside the Hero class to prevent
        // firing this from multiple places. I can't do it right now because
        // I'm running out of time.

        hero.ResetLives();
        signalBus.Fire(new LivesChangedSignal() { Player = hero.Role, Lives = hero.Lives });
        opponent.ResetLives();
        signalBus.Fire(new LivesChangedSignal() { Player = opponent.Role, Lives = opponent.Lives });
        
        Serve();
    }

    public void FinishMatch(Hero loser)
    {
        status = GameplayStatus.End;
        dialogsManager.CreateDialog(DialogType.Win);
        ball.PauseMovement();
        signalBus.Fire(new LivesChangedSignal() { Player = loser.Role, Lives = loser.Lives });

        DeactivateTouch();
        DeactivatePlayerMovement();
    }

    public PlayerType? GetWinner()
    {
        if (status == GameplayStatus.End)
        {
            if (hero.Lives <= 0)
            {
                return PlayerType.Opponent;
            }
            else if (opponent.Lives <= 0)
            {
                return PlayerType.Hero;
            }
        }
        
        return null;
    }
}
