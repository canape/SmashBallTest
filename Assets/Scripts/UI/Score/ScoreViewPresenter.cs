using UnityEngine;
using Zenject;

public class ScoreViewPresenter : IPresenter
{
    private ScoreView view;

    [Inject] SignalBus signalBus;

    public ScoreViewPresenter(ScoreView view)
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
            view.ChangeOpponentText($"<b>Hero:</b> {data.Lives}");
        }
        else
        {
            Debug.LogError("");
        }
    }

    public class Factory : PlaceholderFactory<ScoreView, ScoreViewPresenter> {}
}
