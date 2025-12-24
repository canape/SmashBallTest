using UnityEngine;
using Zenject;

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

    private void OnLivesChange(LivesChangedSignal data)
    {
        
    }

    public class Factory : PlaceholderFactory<SmashView, SmashPresenter> {}
}
