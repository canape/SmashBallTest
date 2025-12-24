using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
using Zenject;

public interface IHeroesManager
{
    public HeroData GetHeroDataById(int heroId);
    public Hero GetHeroById(int heroId);
}

public class HeroesManager : IHeroesManager
{
    private readonly HeroesData heroesData;
    private readonly DiContainer diContainer;

    public HeroesManager(HeroesData heroesData, DiContainer diContainer)
    {
        this.heroesData = heroesData;
        this.diContainer = diContainer;
    }

    public HeroData GetHeroDataById(int heroId)
    {
        return heroesData.Datas.FirstOrDefault(hero => hero.HeroId == heroId);
    }

    public Hero GetHeroById(int heroId)
    {
        var heroData = GetHeroDataById(heroId);
        if (heroData == null)
        {
            Debug.LogError($"HeroData not exist by the heroId {heroId}");
            return null;
        }

        var hero = diContainer.InstantiatePrefabForComponent<Hero>(heroData.Prefab);
        var AoE = CreateAoE(heroData);
        hero.SetAoE(AoE);

        return hero;
    }

    private HeroAoE CreateAoE(HeroData heroData)
    {
        if (heroData.AoEPrefab == null)
        {
            Debug.LogError($"There is no AoEPrefab set in the hero {heroData.HeroName}");
            return null;
        }

        var heroAoE = diContainer.InstantiatePrefabForComponent<HeroAoE>(heroData.AoEPrefab);
        if (heroAoE == null)
        {
            Debug.LogError($"The hero prefab cannot be instantiated for the hero {heroData.HeroName}");
            return null;
        }

        return heroAoE;
    }
}
