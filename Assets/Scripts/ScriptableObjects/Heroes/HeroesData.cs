using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmashBallTest.ScriptableObjects
{
[CreateAssetMenu(fileName = "HeroesData", menuName = "SamshBallTest/HeroesData")]
public class HeroesData : ScriptableObject
{
    [SerializeField] private HeroData[] datas;

    public HeroData[] Datas => datas;
 }
}
