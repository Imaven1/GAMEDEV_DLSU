using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

/// <summary>
/// Plays a list of particle systems at set times, like a timeline.
/// Put this on the parent (e.g. cho_henshin), drag each child particle
/// system into the Steps list, and set when each should start.
/// </summary>
public class Henshin: MonoBehaviour
{
    [Serializable]
    public class Step
    {
        public string label = "Step";
        [Tooltip("The particle system to play (its children play too).")]
        public ParticleSystem system;
        [Tooltip("Seconds after the sequence starts.")]
        public float startTime = 0f;
        [Tooltip("Optional: runs at the same moment. Use it to swap the model, enable objects, etc.")]
        public UnityEvent onStart;
    }

    [Header("Sequence")]
    public Step[] steps;
    public bool playOnStart = true;
    [Tooltip("Press this key to play. Set to None to disable.")]
    public Key triggerKey = Key.Space;
    public bool loop = false;
    [Tooltip("Total length. Only used for looping.")]
    public float sequenceLength = 3f;

    [Header("Camera shake (optional)")]
    public Transform shakeTarget;          // usually the Main Camera
    public float shakeTime = 1.2f;
    public float shakeDuration = 0.25f;
    public float shakeStrength = 0.15f;

    [Header("Enable after sequence (optional)")]
    public GameObject enableAtEnd;         // e.g. the ElementalRings parent
    public float enableAtEndTime = 2f;

    Coroutine running;

    Vector3 shakeOrigin;

    /// <summary>Stops the sequence and clears every effect.</summary>
    public void Stop()
    {
        StopAllCoroutines();
        running = null;
        foreach (var s in steps)
            if (s.system != null)
                s.system.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        if (enableAtEnd != null) enableAtEnd.SetActive(false);
        if (shakeTarget != null) shakeTarget.localPosition = shakeOrigin;
    }

    void Awake()
    {
        if (shakeTarget != null) shakeOrigin = shakeTarget.localPosition;

        // stop everything so nothing plays on its own before the sequence
        foreach (var s in steps)
            if (s.system != null)
                s.system.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        if (enableAtEnd != null) enableAtEnd.SetActive(false);
    }

    void Start()
    {
        if (playOnStart) Play();
    }

    void Update()
    {
        if (triggerKey != Key.None && Keyboard.current != null &&
            Keyboard.current[triggerKey].wasPressedThisFrame)
            Play();
    }

    [ContextMenu("Play")]
    public void Play()
    {
        if (running != null) StopCoroutine(running);
        running = StartCoroutine(Run());
    }

    IEnumerator Run()
    {
        do
        {
            // reset
            foreach (var s in steps)
                if (s.system != null)
                    s.system.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            if (enableAtEnd != null) enableAtEnd.SetActive(false);

            var order = (Step[])steps.Clone();
            Array.Sort(order, (a, b) => a.startTime.CompareTo(b.startTime));

            bool flashed = false, shook = false, enabled = false;
            int next = 0;
            float t = 0f;
            float end = Mathf.Max(sequenceLength, order.Length > 0 ? order[order.Length - 1].startTime : 0f);

            while (t < end || next < order.Length)
            {
                while (next < order.Length && t >= order[next].startTime)
                {
                    var s = order[next++];
                    if (s.system != null) s.system.Play(true);
                    s.onStart?.Invoke();
                }

                if (!shook && shakeTarget != null && t >= shakeTime)
                {
                    StartCoroutine(Shake());
                    shook = true;
                }

                if (!enabled && enableAtEnd != null && t >= enableAtEndTime)
                {
                    enableAtEnd.SetActive(true);
                    enabled = true;
                }

                t += Time.deltaTime;
                yield return null;
            }
        } while (loop);

        running = null;
    }

    IEnumerator Shake()
    {
        Vector3 origin = shakeTarget.localPosition;
        float t = 0f;
        while (t < shakeDuration)
        {
            float falloff = 1f - (t / shakeDuration);
            shakeTarget.localPosition = origin + UnityEngine.Random.insideUnitSphere * shakeStrength * falloff;
            t += Time.deltaTime;
            yield return null;
        }
        shakeTarget.localPosition = origin;
    }
}