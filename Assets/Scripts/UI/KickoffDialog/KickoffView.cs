using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace SmashBallTest.UI
{
    public class KickoffView : IView
    {
        [Inject]
        private PlayerInput playerInput;

        [Inject]
        private KickoffPresenter.Factory presenterFactory;

        private InputAction touchPressInputAction;

        void Awake()
        {
            presenterFactory.Create(this);
            touchPressInputAction = playerInput.actions["Touch"];
        }

        protected override void OnEnable()
        {
            touchPressInputAction.started += TouchStarted;
            base.OnEnable();
        }

        protected override void OnDisable()
        {
            touchPressInputAction.started -= TouchStarted;
            base.OnDisable();
        }

        private void TouchStarted(InputAction.CallbackContext context)
        {
            Destroy(gameObject);
        }
    }
}
