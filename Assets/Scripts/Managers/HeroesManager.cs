using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;

public interface IHeroesManager
{
    public HeroData GetHeroDataById(int heroId);
    public Hero GetHeroById(int heroId);
}

public class HeroesManager : IHeroesManager
{
    private readonly HeroesData heroesData;

    public HeroesManager(HeroesData heroesData)
    {
        this.heroesData = heroesData;
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

        return heroData.GetHero();
    }
}
