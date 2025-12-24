using UnityEngine;
using Zenject;
using SmashBallTest;

namespace SmashBallTest.UI
{
public class WinPresenter : IPresenter
{
    private WinView view;

    [Inject] GamePlayController gameplayController;

    public WinPresenter(WinView view)
    {
        this.view = view;
        view.OnViewEnable += OnViewEnable;

        view.OnPlay += OnPlay;
    }

    private void OnViewEnable()
    {
        var winner = gameplayController.GetWinner();
        view.SetText($"The winner is <br> {winner}");
    }

    private void OnPlay()
    {
        gameplayController.StartMatch();
    }

    public class Factory : PlaceholderFactory<WinView, WinPresenter> {}
}
}
