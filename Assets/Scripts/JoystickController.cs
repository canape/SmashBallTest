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
    //private PlayerInput _playerInput;
    //private InputAction _touchPressInputAction;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        if (_rb == null)
        {
            Debug.LogError($"There is not RigidBody in the JoysticsController for the GameObject with name {name}");
            return;
        }

        /*_playerInput = GetComponent<PlayerInput>();
        if (_playerInput == null)
        {
            Debug.LogError($"There is not PlayerInput in the JoysticsController for the GameObject with name {name}");
            return;
        }

        _touchPressInputAction = _playerInput.actions["Touch"];*/
    }

    void OnEnable()
    {
        //_touchPressInputAction.started += TouchStarted;
        //_touchPressInputAction.canceled += TouchEnded;
    }

    void Start()
    {

    }

    void OnDisable()
    {
        //_touchPressInputAction.started -= TouchStarted;
        //_touchPressInputAction.canceled -= TouchEnded;
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

    /*private void TouchStarted(InputAction.CallbackContext context)
    {
        var touchscreen = Touchscreen.current;
        if (touchscreen == null) return;
        var phase = touchscreen.primaryTouch.phase.ReadValue();
        Debug.Log($"Touch started (phase={phase})");
    }*/

    /*private void TouchEnded(InputAction.CallbackContext context)
    {
        var touchscreen = Touchscreen.current;
        if (touchscreen == null) return;
        var phase = touchscreen.primaryTouch.phase.ReadValue();
        Debug.Log($"Touch ended (phase={phase})");

        var jumpPosition = transform.position + new Vector3(0.0f, 1.0f, 0.0f);
        transform.DOJump(transform.position, .2f, 1, 0.3f);
    }*/
}
