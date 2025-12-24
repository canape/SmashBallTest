using System;
using UnityEngine;
using Zenject;

namespace SmashBallTest.Managers
{
public interface IPlayerManager
{
    public int GetSelectedCourtId();
    public int GetSelectedHeroId();
}

public class PlayerManager : IPlayerManager
{
    private readonly IHeroesManager heroesManager;

    public PlayerManager(IHeroesManager heroesManager)
    {
        this.heroesManager = heroesManager;
    }

    public int GetSelectedCourtId()
    {
        return 0;
    }

    public int GetSelectedHeroId()
    {
        return 1;
    }
}
}
