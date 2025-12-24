using UnityEngine;
using Zenject;

public class KickoffPresenter : IPresenter
{
    private KickoffView view;

    [Inject] SignalBus signalBus;

    public KickoffPresenter(KickoffView view)
    {
        this.view = view;
        view.OnViewEnable += OnViewEnable;
        view.OnViewDisable += OnViewDisable;
    }

    private void OnViewEnable()
    {
        
    }

    private void OnViewDisable()
    {
        
    }

    public class Factory : PlaceholderFactory<KickoffView, KickoffPresenter> {}
}
