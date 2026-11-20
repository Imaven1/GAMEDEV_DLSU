using UnityEngine;

/// <summary>
/// Continuously rotates this GameObject. Attach it to each ring
/// (fire, water, earth, air) and set its own speed in the Inspector.
/// </summary>
public class Spin : MonoBehaviour
{
    [Tooltip("Rotation speed in degrees per second, per axis.")]
    public Vector3 degreesPerSecond = new Vector3(0f, 120f, 0f);

    [Tooltip("Self = spins around the object's own axes (use this for tilted rings). World = spins around the scene's axes.")]
    public Space space = Space.Self;

    void Update()
    {
        transform.Rotate(degreesPerSecond * Time.deltaTime, space);
    }
}