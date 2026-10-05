using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public float startTime = 60f;
    public TMP_Text timerText;
    public TMP_Text messageText;
    public LaserSpawner spawner;
    public PlayerMovement player;   


    float timeLeft;
    bool running;

    void Start()
    {
        timeLeft = startTime;
        running = true;
        messageText.text = "";
    }

    void Update()
    {
        if (!running) return;

        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0f)
        {
            timeLeft = 0f;
            EndGame("Time's up!");
        }

        timerText.text =  Mathf.CeilToInt(timeLeft).ToString();
    }

    public void Win()
    {
        EndGame("You win!");
    }

    public void Lose()
    {
        EndGame("You died!");
    }

    public void AddTime(float seconds)  
    {
        if (running) timeLeft += seconds;
    }

    void EndGame(string message)
    {
        if (!running) return;   
        running = false;
        if (player != null) player.enabled = false;
        messageText.text = message;
        spawner.enabled = false;        
        Invoke(nameof(Restart), 3f);  


    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}