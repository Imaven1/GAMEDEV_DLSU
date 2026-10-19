using UnityEngine;

public class Guard : MonoBehaviour
{
    [Header("Guard")]
    public Transform[] waypoints;
    public float moveSpeed =2f;
    public float turnSpeed = 180f;
    public float waitTime = 1f;




    [Header("Detection")]
    public Transform player;
    PlayerHealth playerHealth;
    public LayerMask playerMask;
    public LayerMask obstructionMask;
    public float senseRadius = 0.3f;
    public float sightRange = 5f;
    [Range(1f, 180f)] public float sightAngle = 60f;
    public float eyeHeight = 1.5f;
    public Vector3 targetOffset = new Vector3(0f, 1f, 0f);

  
    int index;
    float waitUntil;


    void Start()
    {
        playerHealth = player.GetComponentInParent<PlayerHealth>();

        if (playerHealth == null)
            Debug.LogError("PatrolGuard: no PlayerHealth found on the player.");
    }

    // Update is called once per frame
    private void Update()
    {

        Guarding();

        if (isDetectingPlayer())
        {
            playerHealth.TakeDamage(1);
        }
    }

    void Guarding()
    {
        if (waypoints.Length == 0 || Time.time < waitUntil) return;

        Vector3 target = waypoints[index].position;
        target.y = transform.position.y;   // stay at our own height

        Vector3 toTarget = target - transform.position;
        if (toTarget.magnitude < 0.1f)
        {
            index = (index + 1) % waypoints.Length;   // wrap around to the first waypoint
            waitUntil = Time.time + waitTime;
            return;
        }

        Quaternion look = Quaternion.LookRotation(toTarget);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, look, turnSpeed * Time.deltaTime);
        transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
    }

    bool isDetectingPlayer()
    {
        //player is close
        if (Physics.CheckSphere(transform.position, senseRadius, playerMask))
            return true;

        //Player within sight or area
        Vector3 eye = transform.position + Vector3.up * eyeHeight;
        Vector3 toPlayer = (player.position + targetOffset) - eye;

        if (toPlayer.magnitude > sightRange) return false;
        if (Vector3.Angle(transform.forward, toPlayer) > sightAngle / 2f) return false;

        //Player is not obstructed
        if (Physics.Raycast(eye, toPlayer.normalized, out RaycastHit hit, sightRange, playerMask | obstructionMask))
            return ((1 << hit.collider.gameObject.layer) & playerMask) != 0;

        return false;
    }
    
}
