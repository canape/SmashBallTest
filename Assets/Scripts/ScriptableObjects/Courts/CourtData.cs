using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CourtData
{
    [SerializeField] private int courtId;
    [SerializeField] private string courtName;
    [SerializeField] private GameObject prefab;

    public int CourtId => courtId;
    public string CourtName => courtName;

    public Court GetCourt()
    {
        if (prefab == null)
        {
            Debug.LogError($"Prefab not configured in the court {courtName}");
            return null;
        }
        
        var courtGameObject = GameObject.Instantiate(prefab);
        if (courtGameObject == null)
        {
            Debug.LogError($"Cannot instantiate the court {courtName}");
        }

        courtGameObject.name = courtName;

        return courtGameObject.AddComponent<Court>();
    }
}
