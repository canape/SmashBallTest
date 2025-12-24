using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Court : MonoBehaviour
{
    private CourtData courtData;

    private Ball ball;
    private Hero hero;
    private Hero opponent;

    public void Initialize(CourtData courtData)
    {
        this.courtData = courtData;
    }

    public void SetBall(Ball ball)
    {
        if (this.ball != null)
        {
            Destroy(ball.gameObject);
        }

        this.ball = ball;
        this.ball.transform.SetParent(transform, false);
    }

    public void SetHero(Hero hero)
    {
        if (this.hero != null)
        {
            Destroy(this.hero.gameObject);
        }

        this.hero = hero;
        this.hero.Role = PlayerType.Hero;
        this.hero.transform.SetParent(transform, false);
    }

    public void SetOpponent(Hero opponent)
    {
        if (this.opponent != null)
        {
            Destroy(this.opponent.gameObject);
        }

        this.opponent = opponent;
        this.opponent.Role = PlayerType.Opponent;
        this.opponent.transform.SetParent(transform, false);
    }

    public void ResetPositions()
    {
        hero.transform.position = courtData.StartHeroPosition;
        opponent.transform.position = courtData.StartOpponentPosition;
        ball.transform.position = courtData.StartHeroPosition + new Vector3(0, 0.01f, 2);
    }
}
