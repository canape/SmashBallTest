using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogsData", menuName = "SamshBallTest/DialogsData")]
public class DialogsData : ScriptableObject
{
    [SerializeField] private DialogsContainer dialogsContainer;
    [SerializeField] private DialogData[] datas;

    public DialogsContainer DialogsContainer => dialogsContainer;
    public DialogData[] Datas => datas;
}
