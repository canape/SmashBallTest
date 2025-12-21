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
    [SerializeField] private GameObject AoEPrefab;

    public int HeroId => heroId;
    public string HeroName => heroName;
    public int Health => health;

    public Hero GetHero()
    {
        if (prefab == null)
        {
            Debug.LogError($"There is no prefab set in the hero {heroName}");
            return null;
        }

        var heroGameObject = GameObject.Instantiate(prefab);
        if (heroGameObject == null)
        {
            Debug.LogError($"The hero prefab cannot be instantiated for the hero {heroName}");
            return null;
        }

        heroGameObject.name = heroName;

        var hero = heroGameObject.GetComponent<Hero>();
        var AoE = CreateAoE();
        hero.SetAoE(AoE);

        return hero; 
    }

    private HeroAoE CreateAoE()
    {
        if (AoEPrefab == null)
        {
            Debug.LogError($"There is no AoEPrefab set in the hero {heroName}");
            return null;
        }

        var AoEGameObject = GameObject.Instantiate(AoEPrefab);
        if (AoEGameObject == null)
        {
            Debug.LogError($"The hero prefab cannot be instantiated for the hero {heroName}");
            return null;
        }

        return AoEGameObject.AddComponent<HeroAoE>();
    }
}
