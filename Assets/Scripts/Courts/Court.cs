using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Court : MonoBehaviour
{
    private CourtData courtData;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize(CourtData courtData)
    {
        this.courtData = courtData;
    }

    public void SetBall(Ball ball)
    {
        ball.transform.SetParent(transform);
        ball.transform.position = courtData.StartHeroPosition + new Vector3(0, 0.01f, 2);
    }

    public void SetHero(Hero hero)
    {
        hero.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
        hero.transform.SetParent(transform);
        hero.transform.position = courtData.StartHeroPosition;
    }

    public void SetOpponent(Hero opponent)
    {
        opponent.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
        opponent.transform.SetParent(transform);
        opponent.transform.position = courtData.StartOpponentPosition;
        
        opponent.GetComponent<JoysticsController>().enabled = false;
    }
}
