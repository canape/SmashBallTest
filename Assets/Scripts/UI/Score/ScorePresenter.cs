using UnityEngine;
using Zenject;
using SmashBallTest.Installers;

namespace SmashBallTest.UI
{
    public class ScorePresenter : IPresenter
    {
        private ScoreView view;

        [Inject] SignalBus signalBus;

        public ScorePresenter(ScoreView view)
        {
            this.view = view;
            view.OnViewEnable += OnViewEnable;
            view.OnViewDisable += OnViewDisable;
        }

        private void OnViewEnable()
        {
            signalBus.Subscribe<LivesChangedSignal>(OnLivesChange);
        }

        private void OnViewDisable()
        {
            signalBus.Unsubscribe<LivesChangedSignal>(OnLivesChange);
        }

        private void OnLivesChange(LivesChangedSignal data)
        {
            if (data.Player == PlayerType.Hero)
            {
                view.ChangeHeroText($"<b>Hero:</b> {data.Lives}");
            }
            else if (data.Player == PlayerType.Opponent)
            {
                view.ChangeOpponentText($"<b>Opponent:</b> {data.Lives}");
            }
            else
            {
                Debug.LogError("");
            }
        }

        public class Factory : PlaceholderFactory<ScoreView, ScorePresenter> {}
    }
}
