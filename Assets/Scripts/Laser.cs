using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Laser : MonoBehaviour
{
    public float speed = 6f;
    public int damage = 1;
    float remaining;   

    Vector3 direction;

    void Awake()
    {
        // Kinematic Rigidbody so trigger events fire reliably on a moving object
        var rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;
    }

    

    public void Init(Vector3 moveDirection, float travelDistance)
    {
        direction = moveDirection.normalized;
        remaining = travelDistance;
    }

    void Update()
    {
        float step = speed * Time.deltaTime;
        transform.position += direction * step;

        remaining -= step;
        if (remaining <= 0f)
            Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerHealth health))
        {
            health.TakeDamage(damage);
        }
    }
}