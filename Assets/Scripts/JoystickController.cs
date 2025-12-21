using UnityEngine;
using UnityEngine.InputSystem;
using Terresquall;
using System.Threading;
using DG.Tweening;

public class JoysticsController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private Vector3 _movement;
    private Rigidbody _rb;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        if (_rb == null)
        {
            Debug.LogError($"There is not RigidBody in the JoysticsController for the GameObject with name {name}");
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
        _rb.velocity = _movement * moveSpeed;
    }
}
