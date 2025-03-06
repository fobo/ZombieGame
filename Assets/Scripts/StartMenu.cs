using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    void Start()
    {
        if (LootTableManager.Instance != null) Destroy(LootTableManager.Instance.gameObject);
        if (HUDController.Instance != null) Destroy(HUDController.Instance.gameObject);
        if (LevelManager.Instance != null) Destroy(LevelManager.Instance.gameObject);
        if (EventBus.Instance != null) Destroy(EventBus.Instance.gameObject);
        if (GameDirector.Instance != null) Destroy(GameDirector.Instance.gameObject);
        if (GameStateManager.Instance != null) Destroy(GameStateManager.Instance.gameObject);
        if (InventorySystem.Instance != null) Destroy(InventorySystem.Instance.gameObject);
        if (MomentoSystem.Instance != null) Destroy(MomentoSystem.Instance.gameObject);
    }
    public void LoadNextLevel()
    {
        // Reset difficulty if you are going to the next scene
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
    }
}
