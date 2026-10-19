using UnityEngine;
using UnityEngine.InputSystem;

public class Flashlight : MonoBehaviour
{
    public LayerMask ObstructionMask;
    public bool turnOn = true;

    Light lightComp;

    public bool isOn => lightComp.enabled;

    void Awake()
    {
        lightComp = GetComponent<Light>();
        lightComp.enabled = turnOn; 
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.fKey.wasPressedThisFrame)
            lightComp.enabled = !lightComp.enabled;
    }

//Takes in a point x and returns true if point x is illuminated by the flashlight object.
    public bool isIlluminated(Vector3 point)
    {
        //flashlight is off
        if (!isOn) return false;

        //of of flashlight's range
        Vector3 toPoint = point - transform.position;
        float distance = toPoint.magnitude;

        if (distance > lightComp.range) return false;
        if (Vector3.Angle(transform.forward, toPoint) > lightComp.spotAngle / 2f) return false;


       //flashlight is obstructed 
       if (Physics.Raycast(transform.position, toPoint.normalized, distance, ObstructionMask)) return false;

        return true;
    }
}
