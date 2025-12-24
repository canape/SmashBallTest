using System;
using System.Linq;
using UnityEngine;
using Zenject;
using SmashBallTest.ScriptableObjects;
using SmashBallTest.UI;

namespace SmashBallTest.Managers
{
public interface IDialogsManager
{
    public void CreateDialog(DialogType dialogType);
}

public class DialogsManager : IDialogsManager
{
    private readonly DialogsData dialogsData;
    private readonly DialogsContainer dialogsContainer;
    private readonly DiContainer diContainer;

    public DialogsManager(DiContainer diContainer, DialogsData dialogsData)
    {
        this.diContainer = diContainer;
        this.dialogsData = dialogsData;
        dialogsContainer = GameObject.Instantiate(dialogsData.DialogsContainer);
    }

    private GameObject GetDialogPrefabDataByType(DialogType type)
    {
        var dialogData = dialogsData.Datas.FirstOrDefault(dialogData => dialogData.DialogType == type);
        return dialogData?.DialogPrefab;
    }

    public void CreateDialog(DialogType dialogType)
    {
        var prefab = GetDialogPrefabDataByType(dialogType);
        if (prefab == null)
        {
            Debug.LogError($"The prefab for the dialog {dialogType} doesn't exist");
            return;
        }

        diContainer.InstantiatePrefab(prefab, dialogsContainer.transform);
    }
}
}
