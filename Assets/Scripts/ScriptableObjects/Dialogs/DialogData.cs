using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmashBallTest.ScriptableObjects
{
    [Serializable]
    public class DialogData
    {
        [SerializeField] private DialogType dialogType;
        [SerializeField] private GameObject dialogPrefab;

        public DialogType DialogType => dialogType;
        public GameObject DialogPrefab => dialogPrefab;

    }
}
