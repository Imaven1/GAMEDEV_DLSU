using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class BallThrower : MonoBehaviour
{
    
    private GameObject ball;

    [SerializeField]
    float ballThrowSpeed = 50f;

    private bool thrown, holding;
    private Vector3 newPosition;

    int ballsThrown;

    Rigidbody rb;

    
    void Start()
    {
        ballSetup();
    }

    private void ballSetup()
    {
        GameObject _ball = GameObject.FindWithTag("Player");
        ball = _ball;
        rb = ball.GetComponent<Rigidbody>();
        resetBall();
    }

    void resetBall()
    {
        ball.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        ball.GetComponent<Rigidbody>().useGravity = false;

        thrown = false;
        holding = true;
    }

    private void Update()
    {
        if (thrown)
            return;
        if (holding)
            pickupBall();

        if (Mouse.current.leftButton.isPressed)
        {
            Vector3 direction = Camera.main.transform.forward;
            ball.GetComponent<Rigidbody>().useGravity = true;
            rb.AddForce(direction * ballThrowSpeed, ForceMode.Impulse);
            thrown = true;
            holding = false;
            Invoke("resetBall", 3f);
            ballsThrown++;
        }
    }

    void pickupBall()
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();
        mousePos.z = Camera.main.nearClipPlane * 15f;
        newPosition = Camera.main.ScreenToWorldPoint(mousePos);
        ball.transform.localPosition = Vector3.Lerp(ball.transform.localPosition, newPosition, 80f * Time.deltaTime);
    }



}
