using TMPro;
using UnityEngine;
using Zenject;

namespace SmashBallTest.UI
{
public class ScoreView : IView
{
    [SerializeField] private TextMeshProUGUI heroScore;
    [SerializeField] private TextMeshProUGUI opponentScore;

    [Inject] ScorePresenter.Factory presenterFactory;

    void Awake()
    {
        presenterFactory.Create(this);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ChangeHeroText(string text)
    {
        heroScore.text = text;
    }

    public void ChangeOpponentText(string text)
    {
        opponentScore.text = text;
    }
}
}
