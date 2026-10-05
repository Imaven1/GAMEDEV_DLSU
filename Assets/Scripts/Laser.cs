using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Laser : MonoBehaviour
{
    public float speed = 6f;
    public int damage = 1;

    Vector3 direction;

    void Awake()
    {
        // Kinematic Rigidbody so trigger events fire reliably on a moving object
        var rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;
    }

    // Called by the spawner right after creating the laser
    public void Init(Vector3 moveDirection, float travelDistance)
    {
        direction = moveDirection.normalized;
        Destroy(gameObject, travelDistance / speed + 1f); // clean up after it passes the start
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerHealth health))
        {
            health.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}