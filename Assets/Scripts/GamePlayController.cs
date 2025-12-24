using System;
using Zenject;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;
using Terresquall;
using SmashBallTest.Courts;
using SmashBallTest.Heroes;
using SmashBallTest.Installers;
using SmashBallTest.Managers;
using SmashBallTest.ScriptableObjects;


#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SmashBallTest
{
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

        private PlayerInput playerInput;
        private InputAction touchPressInputAction;
        
        private Court court;
        private Ball ball;
        private Hero hero;
        private Hero opponent;

        private bool touchActive;
        private bool movementActive;

        private GameplayStatus status;

        //This doesn't mneeds to be here
        bool opponentAlreadySwing;

        public GamePlayController(
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
            touchPressInputAction.canceled += TouchEnded;

            SetupCameras();
            StartMatch();
        }

        public void Dispose()
        {
            touchPressInputAction.canceled -= TouchEnded;
        }

        public void Tick()
        {
            if (status != GameplayStatus.Point)
            {
                return;
            }

            AwesomeOpponentAI();
            DetectHit(hero);
            DetectHit(opponent);
            MoveHero();
        }

        private void AwesomeOpponentAI()
        {
            float distance = Vector3.Distance(opponent.transform.position, ball.transform.position);

            if (!opponent.IsSwining && distance <= 2f)
            {
                opponent.Swing();

                bool swingFailed = Random.Range(0, 100) < 10;
                if (swingFailed)
                {
                    return;
                }

                HitTheBall(opponent.transform.position);
            }
        }

        void HitTheBall(Vector3 position)
        {
            Vector3 hitNormal = (ball.transform.position - position).normalized;
            float forceMagnitude = 5;
            ball.SetDirectionAndForce(hitNormal, forceMagnitude);
        }

        private void DetectHit(Hero undefinedHero)
        {
            float distance = Vector3.Distance(undefinedHero.transform.position, ball.transform.position);
            if (distance <= 0.7f)
            {
                undefinedHero.SubstractLive();

                if (undefinedHero.Lives <= 0)
                {
                    FinishMatch(undefinedHero);
                }
                else
                {
                    FinishPoint(undefinedHero);
                }
            }
        }

        private void MoveHero()
        {
            if (!movementActive)
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

        private void TouchEnded(InputAction.CallbackContext context)
        {
            if (!touchActive)
            {
                return;
            }

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
                    movementActive = true;
                }

                HitTheBall(hero.transform.position);
            }
        }

        public void Serve()
        {
            status = GameplayStatus.Serve;
            dialogsManager.CreateDialog(DialogType.Kickoff);
            court.ResetPositions();
            ball.PauseMovement();

            touchActive = true;
            movementActive = false;
        }

        public void FinishPoint(Hero loser)
        {
            status = GameplayStatus.Smash;
            dialogsManager.CreateDialog(DialogType.Smash);
            ball.PauseMovement();

            touchActive = false;
            movementActive = false;
        }

        public void StartMatch()
        {
            hero.ResetLives();
            opponent.ResetLives();
            
            Serve();
        }

        public void FinishMatch(Hero loser)
        {
            status = GameplayStatus.End;
            dialogsManager.CreateDialog(DialogType.Win);
            ball.PauseMovement();

            touchActive = false;
            movementActive = false;
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
}
