using UnityEngine;

public class WinScreen : MonoBehaviour
{
    [SerializeField] private GameObject winPanel;
    private bool hasWon;

    private void Start()
    {
        winPanel.SetActive(false);
    }

    public void ShowWin()
    {
        if (hasWon) return;  
        hasWon = true;

        winPanel.SetActive(true);
        Time.timeScale = 0f;  
    }
}