using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError($"The ball {name} doesn't have a RigidBody");
            return;
        }
    }

    public void SetDirectionAndForce(Vector3 direction, float force)
    {
        rb.velocity = Vector3.zero;
        rb.AddForce(direction * force, ForceMode.Impulse);
    }
}
