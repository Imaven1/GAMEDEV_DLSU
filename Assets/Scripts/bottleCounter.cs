using System.Reflection.Emit;
using UnityEngine;
using TMPro;

using System;



public class bottleCounter : MonoBehaviour
{
    [SerializeField] private EmptyDetector detector;
    [SerializeField] private TMP_Text label;
    private void OnEnable()
    {
        detector.onCountChanged.AddListener(UpdateLabel);
        UpdateLabel(detector.Count);

    }

    private void OnDisable()
    {
        detector.onCountChanged.RemoveListener(UpdateLabel);
    }

    private void UpdateLabel(int count)
    {
        label.text = $"Cans left: {count} ";
    }

}
