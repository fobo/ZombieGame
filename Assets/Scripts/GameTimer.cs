using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public float totalTime = 420f; // 7 minutes in seconds
    private float currentTime;
    public TextMeshProUGUI timerText;
    public GameObject levelSelectMenu;

    private void Start()
    {
        currentTime = totalTime;
        levelSelectMenu.SetActive(false); // Hide menu initially
    }

    private void Update()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            UpdateTimerUI();
        }
        else
        {
            EndNightCycle();
        }
    }

    private void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    private void EndNightCycle()
    {
        currentTime = 0;
        Time.timeScale = 0; // Pause the game
        levelSelectMenu.SetActive(true); // Show level select menu
    }
    public void RevisitLevel()
    {
        Time.timeScale = 1; // Resume time
        LevelManager.Instance.RevisitLevel();
    }

    public void NextLevel()
    {
        Time.timeScale = 1; // Resume time
        LevelManager.Instance.LoadNextLevel();
    }

}
