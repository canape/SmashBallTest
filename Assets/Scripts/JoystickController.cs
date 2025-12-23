using UnityEngine;
using UnityEngine.InputSystem;
using Terresquall;
using System.Threading;
using DG.Tweening;
using Unity.VisualScripting;

public class JoysticsController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private Vector3 _movement;
    private Rigidbody _rb;
    private Hero hero;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        if (_rb == null)
        {
            Debug.LogError($"There is not RigidBody in the JoysticsController for the GameObject with name {name}");
            return;
        }

        hero = GetComponent<Hero>();
        if (hero == null)
        {
            Debug.LogError($"There is not Hero in the JoysticsController for the GameObject with name {name}");
            return;
        }
    }

    void OnEnable()
    {
    }

    void Start()
    {

    }

    void OnDisable()
    {
    }

    void Update()
    {
        float moveX = VirtualJoystick.GetAxis("Horizontal");
        float moveZ = VirtualJoystick.GetAxis("Vertical");

        _movement = new Vector3(moveX, 0, moveZ).normalized;
    }

    void FixedUpdate()
    {
        if (_movement == Vector3.zero)
        {
            _rb.velocity = Vector3.zero;
            return;
        }

        _rb.velocity = _movement * moveSpeed;
        hero.SetDirection(_movement);
    }
}
