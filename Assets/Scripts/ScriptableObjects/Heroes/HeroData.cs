using System;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class HeroData
{
    [SerializeField] private int heroId;
    [SerializeField] private string heroName;
    [SerializeField] private int health;
    [SerializeField] private GameObject prefab;
    [SerializeField] private GameObject aoEPrefab;

    public int HeroId => heroId;
    public string HeroName => heroName;
    public int Health => health;
    public GameObject Prefab => prefab;
    public GameObject AoEPrefab => aoEPrefab;
}
