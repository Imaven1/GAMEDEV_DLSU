using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SimplePlatform : MonoBehaviour
{
    [SerializeField] private float distance = 5f;  
    [SerializeField] private float speed = 0;  

    private Rigidbody rb;
    private Vector3 startPos;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    private void Start()
    {
        startPos = transform.position;
    }

    private void FixedUpdate()
    {
        float x = Mathf.Sin(Time.time * speed) * distance;
        rb.MovePosition(startPos + new Vector3(0f, 0f, x));
    }
}