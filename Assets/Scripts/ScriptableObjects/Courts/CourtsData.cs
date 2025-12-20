using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CourtsData", menuName = "SamshBallTest/CourtsData")]
public class CourtsData : ScriptableObject
{
    [SerializeField] private CourtData[] datas;
    public CourtData[] Datas => datas;
}
