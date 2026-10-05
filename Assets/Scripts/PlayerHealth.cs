using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CharacterController))]
public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    public Transform startPoint;          
    public float invulnerableTime = 1f;   

    public UnityEvent<int> onHealthChanged; 

    int currentHealth;
    float invulnerableUntil;
    CharacterController controller;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        currentHealth = maxHealth;
    }

    void Start()
    {
        onHealthChanged?.Invoke(currentHealth);
    }

    public void TakeDamage(int amount)
    {
        if (Time.time < invulnerableUntil) return;

        currentHealth = Mathf.Max(0, currentHealth - amount);
        invulnerableUntil = Time.time + invulnerableTime;
        onHealthChanged?.Invoke(currentHealth);

        if (currentHealth <= 0)
            ResetPlayer();
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
        onHealthChanged?.Invoke(currentHealth);
    }

    void ResetPlayer()
    { 
        controller.enabled = false;
        transform.SetPositionAndRotation(startPoint.position, startPoint.rotation);
        controller.enabled = true;

        currentHealth = maxHealth;
        onHealthChanged?.Invoke(currentHealth);
    }
}