using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CourtData
{
    [SerializeField] private int courtId;
    [SerializeField] private string courtName;
    [SerializeField] private Vector3 startHeroPosition;
    [SerializeField] private Vector3 startOpponentPosition;
    [SerializeField] private GameObject prefab;
    [SerializeField] private GameObject ballPrefab;

    public int CourtId => courtId;
    public string CourtName => courtName;
    public Vector3 StartHeroPosition => startHeroPosition;
    public Vector3 StartOpponentPosition => startOpponentPosition;

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

        var court = courtGameObject.AddComponent<Court>();
        court.Initialize(this);

        return court;
    }

    public Ball GetBall()
    {
        if (ballPrefab == null)
        {
            Debug.LogError($"ballPrefab not configured in the court {courtName}");
            return null;
        }
        
        var ballGameObject = GameObject.Instantiate(ballPrefab);
        if (ballGameObject == null)
        {
            Debug.LogError($"Cannot instantiate the ball in {courtName}");
        }

        ballGameObject.name = "Ball";

        return ballGameObject.AddComponent<Ball>();
    }
}
