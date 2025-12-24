using System.Collections;
using TMPro;
using UnityEngine;
using Zenject;

namespace SmashBallTest.UI
{
public class SmashView : IView
{
    [SerializeField] private float secondsToClose = 2;

    [Inject] 
    SmashPresenter.Factory presenterFactory;

    void Awake()
    {
        presenterFactory.Create(this);
        StartCoroutine(DestroyAfterSomeSeconds());
    }

    private IEnumerator DestroyAfterSomeSeconds()
    {
        yield return new WaitForSeconds(secondsToClose);
        Destroy(gameObject);
    }
}
}
