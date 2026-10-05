using UnityEngine;
using TMPro;

public class HealthUI : MonoBehaviour
{
    public TMP_Text healthText;

    public void UpdateHealth(int health)
    {
        healthText.text = "Health: " + health;
    }
}