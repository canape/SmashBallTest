using UnityEngine;
using DG.Tweening;

public class Hero : MonoBehaviour
{
    [SerializeField] private GameObject character;
    
    private HeroAoE AoE;
    private bool isSwining;
    private int lives;
    private Rigidbody rb;

    public int Lives => lives;
    public PlayerType Role;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError($"There is not RigidBody in the JoysticsController for the GameObject with name {name}");
            return;
        }
        
        ResetLives();
    }

    public void SetAoE(HeroAoE AoE)
    {
        this.AoE = AoE;
        this.AoE.transform.SetParent(transform);
        this.AoE.transform.position = new Vector3(0, 0.01f, 0);
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

    public void Move(Vector3 direction, float speed)
    {
        if (direction == Vector3.zero || speed == 0)
        {
            rb.velocity = Vector3.zero;
            return;
        }
        
        rb.velocity = direction * speed;
        ModifyCharacterRotation(direction);
    }

    public void ModifyCharacterRotation(Vector3 direction)
    {
        Quaternion targetRot = Quaternion.LookRotation(-direction);
        character.transform.rotation = targetRot;
    }

    public void SubstractLive()
    {
        lives--;
    }

    public void ResetLives()
    {
        lives = 3;
    }

    public void ResetRotation()
    {
        //This is not good. It is needed to refactor
        if (Role == PlayerType.Opponent)
        {
            character.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
        else
        {
            character.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }
        
    }
}
