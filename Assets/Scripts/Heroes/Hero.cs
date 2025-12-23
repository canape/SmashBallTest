using UnityEngine;
using DG.Tweening;

public class Hero : MonoBehaviour
{
    [SerializeField] private GameObject character;
    
    private HeroAoE AoE;
    private bool isSwining;
    private int lives;

    public int Lives => lives;

    void Awake()
    {
        lives = 3;
    }

    public void SetAoE(HeroAoE AoE)
    {
        this.AoE = AoE;
        this.AoE.transform.SetParent(transform);
        this.AoE.transform.SetPositionAndRotation(new Vector3(0, 0.01f, 0), Quaternion.Euler(new Vector3 (90.0f, 0, 0)));
    }

    public void Swing()
    {
        if (isSwining)
        {
            return;
        }

        isSwining = true;
        character.transform.DOLocalJump(character.transform.localPosition, .2f, 1, 0.3f).OnComplete(() => {
            isSwining = false;
        });
    }

    public void SetDirection(Vector3 direction)
    {
        Quaternion targetRot = Quaternion.LookRotation(-direction);
        character.transform.rotation = targetRot;
    }

    public void SubstractLive()
    {
        lives--;
    }
}
