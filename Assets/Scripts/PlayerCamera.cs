using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    public float sensX = 0.1f;
    public float sensY = 0.1f;

    public Transform orientation; // optional

    float xRotation;
    float yRotation;
    InputAction lookAction;

    void Awake()
    {
        lookAction = new InputAction("Look", InputActionType.Value, "<Mouse>/delta");
    }

    void OnEnable() { lookAction.Enable(); }
    void OnDisable() { lookAction.Disable(); }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Vector2 look = lookAction.ReadValue<Vector2>();

        yRotation += look.x * sensX;
        xRotation = Mathf.Clamp(xRotation - look.y * sensY, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);

        if (orientation != null)
            orientation.rotation = Quaternion.Euler(0f, yRotation, 0f);
    }
}