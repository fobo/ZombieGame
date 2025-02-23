using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public float totalTime = 420f; // 7 minutes in seconds
    private float currentTime;
    public TextMeshProUGUI timerText;
    public GameObject levelSelectMenu;

    private bool hasEnded = false; // Prevent multiple calls to EndNightCycle

    private void Start()
    {
        currentTime = totalTime;
        levelSelectMenu.SetActive(false); // Hide menu initially
        UpdateTimerUI(); // Ensure UI shows full time at start
    }

    private void Update()
    {
        if (hasEnded) return;

        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            currentTime = Mathf.Max(currentTime, 0); // Clamp to prevent negatives
            UpdateTimerUI();
        }

        if (currentTime <= 0 && !hasEnded)
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
        if (hasEnded) return;

        hasEnded = true;
        currentTime = 0;
        UpdateTimerUI();

        Debug.Log("Pausing the game for Level Select");
        GameStateManager.Instance.SetState(GameState.LevelSelect);
        levelSelectMenu.SetActive(true);
    }


    // Button Methods
    public void RevisitLevel()
    {
        ResetTimer();
        LevelManager.Instance.RevisitLevel();
    }

    public void NextLevel()
    {
        ResetTimer();
        LevelManager.Instance.LoadNextLevel();
    }

    private void ResetTimer()
    {
        currentTime = totalTime;
        hasEnded = false;
        Time.timeScale = 1; // Resume time
        levelSelectMenu.SetActive(false); // Hide menu
        UpdateTimerUI(); // Reset UI
    }
}
