using System;
using Zenject;
using UnityEngine;

public class GamePlayController : IInitializable, IDisposable, ITickable
{
    private readonly IPlayerManager playerManager;
    private readonly ICourtsManager courtManager;
    private readonly IHeroesManager heroesManager;

    private Court court;
    private Hero hero;
    private Hero opponent;

    public GamePlayController(IPlayerManager playerManager, ICourtsManager courtManager, IHeroesManager heroesManager)
    {
        this.playerManager = playerManager;
        this.courtManager = courtManager;
        this.heroesManager = heroesManager;
    }

    public void Initialize()
    {
        var selectCourtId = playerManager.GetSelectedHeroId();
        var selectedHeroId = playerManager.GetSelectedHeroId();

        court = courtManager.GetCourtById(selectCourtId);
        
        hero = heroesManager.GetHeroById(selectedHeroId);
        hero.transform.SetParent(court.transform, false);
        
        opponent = heroesManager.GetHeroById(0);
        opponent.transform.SetParent(court.transform, false);
    }

    public void Dispose()
    {
        //Debug.Log("Dispose");
    }

    public void Tick()
    {
        //Debug.Log("Tick");
    }
}
