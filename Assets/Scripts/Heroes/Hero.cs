using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Hero : MonoBehaviour
{
    [SerializeField] private GameObject character;
    
    private HeroAoE AoE;
    private bool isSwining;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
