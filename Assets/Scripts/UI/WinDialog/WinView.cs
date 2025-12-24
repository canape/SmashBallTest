using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SmashBallTest.UI
{
public class WinView : IView
{
    [SerializeField] private TextMeshProUGUI winnerText;
    [SerializeField] private Button playButton;

    [Inject] WinPresenter.Factory presenterFactor;

    public Action OnPlay;

    void Awake()
    {
        presenterFactor.Create(this);
    }

    protected override void OnEnable()
    {
        playButton.onClick.AddListener(OnPlayButtonClicked);

        base.OnEnable();
    }

    private void OnPlayButtonClicked()
    {
        OnPlay?.Invoke();
        Destroy(gameObject);
    }

    public void SetText(string text)
    {
        winnerText.text = text;
    }
}
}
