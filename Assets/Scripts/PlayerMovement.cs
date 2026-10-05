using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 5f;
    public float jumpHeight = 1.2f;
    public float gravity = -20f;
    public float turnSmoothTime = 0.1f;

    CharacterController controller;
    Transform cam;
    Vector3 velocity;
    float turnVelocity;

    InputAction moveAction;
    InputAction jumpAction;

    void Awake()
    {
        controller = GetComponent<CharacterController>();

        var playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
    }

    void Start()
    {
        // Only used as a direction reference. The camera itself is handled elsewhere.
        cam = Camera.main.transform;
    }

    void Update()
    {
        bool grounded = controller.isGrounded;
        if (grounded && velocity.y < 0f)
            velocity.y = -2f;

        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 dir = new Vector3(input.x, 0f, input.y);

        if (dir.sqrMagnitude > 0.01f)
        {
            float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir * Mathf.Min(dir.magnitude, 1f) * walkSpeed * Time.deltaTime);
        }

        if (grounded && jumpAction.WasPressedThisFrame())
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}