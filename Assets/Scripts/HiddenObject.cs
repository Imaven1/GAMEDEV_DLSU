using UnityEngine;

public class HiddenObject : MonoBehaviour
{
    public Vector3 centerOffset = Vector3.zero;   // adjust if the pivot isn't in the middle

    Flashlight flashlight;
    Renderer[] renderers;

    void Start()
    {
        flashlight = FindAnyObjectByType<Flashlight>();
        renderers = GetComponentsInChildren<Renderer>();

        if (flashlight == null)
            Debug.LogError("HiddenObject: no Flashlight found in the scene.");
    }

    void Update()
    {
        if (flashlight == null) return;

        bool lit = flashlight.isIlluminated(transform.position + centerOffset);

        foreach (Renderer r in renderers)
            r.enabled = lit;
    }
}