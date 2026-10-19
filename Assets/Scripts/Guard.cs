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
    public LayerMask playerMask;
    public LayerMask obstructionMask;
    public float senseRadius = 3f;
    public float sightRadius = 10f;
    [Range(1f, 180f)] public float sightAngle = 60f;
    public float eyeHeight = 1.5f;
    public Vector3 targetOffset = new Vector3(0f, 1f, 0f);

    public GameTimer gameTimer;

    int index;
    float waitUntil;
    bool detected;



    // Update is called once per frame
    private void Update()
    {
        if (detected) return;

        Guarding();

        if (isDetectingPlayer())
        {
            detected = true;
            gameTimer.Lose();
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
        
        //Player is not obstructed
        if (Physics.Raycast(eye, toPlayer.normalized, out RaycastHit hit, sightRadius, playerMask | obstructionMask))
            return ((1 << hit.collider.gameObject.layer) & playerMask) != 0;

        return false;
    }
    
}
