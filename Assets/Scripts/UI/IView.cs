using System;
using UnityEngine;

public abstract class IView : MonoBehaviour
{
    public Action OnViewEnable;
    public Action OnViewStart;
    public Action OnViewDisable;
    public Action OnViewDestroy;

    protected virtual void OnEnable()
    {
        OnViewEnable?.Invoke();
    }

    protected virtual void Start()
    {
        OnViewStart?.Invoke();
    }

    protected virtual void OnDisable()
    {
        OnViewDisable?.Invoke();
    }

    protected virtual void OnDestroy()
    {
        OnViewDestroy?.Invoke();

        OnViewEnable = null;
        OnViewStart = null;
        OnViewDisable = null;
        OnViewDestroy = null;
    }
}
