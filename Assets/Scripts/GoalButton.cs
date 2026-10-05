using UnityEngine;

public class GoalButton : MonoBehaviour
{
    public GameTimer timer;

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<PlayerHealth>() != null)
            timer.Win();
    }
}