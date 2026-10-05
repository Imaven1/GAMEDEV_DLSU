using UnityEngine;

public class FloorPanel : MonoBehaviour
{
    public enum BonusType { Heal, ExtraTime }

    public BonusType bonus;
    public int healAmount = 1;
    public float extraSeconds = 5f;
    public GameTimer timer;

    bool used;

    void OnTriggerEnter(Collider other)
    {
        if (used) return;

        PlayerHealth health = other.GetComponentInParent<PlayerHealth>();
        if (health == null) return;   

        switch (bonus)
        {
            case BonusType.Heal:
                if (health.getCurrentHealth() < health.maxHealth)
                {
                    health.Heal(healAmount);
                }
                break;
            case BonusType.ExtraTime:
                timer.AddTime(extraSeconds);
                break;
        }

        used = true;
        gameObject.SetActive(false);   
    }
}