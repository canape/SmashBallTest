using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HeroesData", menuName = "SamshBallTest/HeroesData")]
public class HeroesData : ScriptableObject
{
    [SerializeField] private HeroData[] datas;

    public HeroData[] Datas => datas;
 }
