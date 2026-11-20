using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Lets you start either the henshin sequence or the avatar (elemental rings)
/// effect on demand. Starting one stops the other.
/// Uses the new Input System. Put this on any GameObject (e.g. "EffectController").
/// </summary>
public class EffectSelector : MonoBehaviour
{
    [Header("Henshin")]
    public Henshin henshin;
    public Key henshinKey = Key.Digit1;

    [Header("Avatar effect")]
    [Tooltip("The parent object holding the fire, water, earth and air rings.")]
    public GameObject avatarRoot;
    public Key avatarKey = Key.Digit2;

    [Header("Stop everything")]
    public Key stopKey = Key.Digit0;

    void Start()
    {
        // make sure nothing is running until you ask for it
        if (henshin != null) henshin.playOnStart = false;
        StopAll();
    }

    void Update()
    {
        var kb = Keyboard.current;
        if (kb == null) return;

        if (kb[henshinKey].wasPressedThisFrame) PlayHenshin();
        if (kb[avatarKey].wasPressedThisFrame) PlayAvatar();
        if (kb[stopKey].wasPressedThisFrame) StopAll();
    }

    // These are public so you can also hook them to UI buttons (OnClick).
    public void PlayHenshin()
    {
        if (avatarRoot != null) avatarRoot.SetActive(false);
        if (henshin != null) henshin.Play();
    }

    public void PlayAvatar()
    {
        if (henshin != null) henshin.Stop();
        if (avatarRoot != null)
        {
            // toggle off then on so pressing again restarts the effect
            avatarRoot.SetActive(false);
            avatarRoot.SetActive(true);
        }
    }

    public void StopAll()
    {
        if (henshin != null) henshin.Stop();
        if (avatarRoot != null) avatarRoot.SetActive(false);
    }
}