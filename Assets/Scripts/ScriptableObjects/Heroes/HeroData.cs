using System;
using UnityEngine;

[Serializable]
public class HeroData
{
    [SerializeField] private int heroId;
    [SerializeField] private string heroName;
    [SerializeField] private int health;
    [SerializeField] private GameObject prefab;

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

        return heroGameObject.AddComponent<Hero>();
    }
}
