using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    public float invulnerableTime = 1f;
    public GameTimer gameTimer;

    public UnityEvent<int> onHealthChanged;

    int currentHealth;
    float invulnerableUntil;
    bool dead;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    void Start()
    {
        onHealthChanged?.Invoke(currentHealth);
    }

    public void TakeDamage(int amount)
    {
        if (dead || Time.time < invulnerableUntil) return;

        currentHealth = Mathf.Max(0, currentHealth - amount);
        invulnerableUntil = Time.time + invulnerableTime;
        onHealthChanged?.Invoke(currentHealth);

        if (currentHealth <= 0)
        {
            dead = true;
            gameTimer.Lose();
        }
    }

    public void Heal(int amount)
    {
        if (dead) return;

        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
        onHealthChanged?.Invoke(currentHealth);
    }
}