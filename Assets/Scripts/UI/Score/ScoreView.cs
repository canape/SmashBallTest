using TMPro;
using UnityEngine;
using Zenject;

public class ScoreView : IView
{
    [SerializeField] private TextMeshProUGUI heroScore;
    [SerializeField] private TextMeshProUGUI opponentScore;

    [Inject] ScoreViewPresenter.Factory _factory;

    void Awake()
    {
        _factory.Create(this);
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
