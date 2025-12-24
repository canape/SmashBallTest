using UnityEngine;
using Zenject;
using SmashBallTest;

namespace SmashBallTest.UI
{
    public class SmashPresenter : IPresenter
    {
        private SmashView view;

        [Inject] 
        GamePlayController gamePlayController;

        public SmashPresenter(SmashView view)
        {
            this.view = view;
            view.OnViewDisable += OnViewDisable;
        }

        private void OnViewDisable()
        {
            gamePlayController.Serve();
        }

        public class Factory : PlaceholderFactory<SmashView, SmashPresenter> {}
    }
}
