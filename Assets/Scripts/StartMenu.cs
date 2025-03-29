using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    void Start()
    {
        Time.timeScale = 1;//make sure the game is running


        //destroy all the managers and controllers if we are restarting the game
        //this code really sucks super badly
        if (GameController.Instance != null) Destroy(GameController.Instance.gameObject);
        if (LootTableManager.Instance != null) Destroy(LootTableManager.Instance.gameObject);
        if (HUDController.Instance != null) Destroy(HUDController.Instance.gameObject);
        if (LevelManager.Instance != null) Destroy(LevelManager.Instance.gameObject);
        if (EventBus.Instance != null) Destroy(EventBus.Instance.gameObject);
        if (GameDirector.Instance != null) Destroy(GameDirector.Instance.gameObject);
        if (InventorySystem.Instance != null) Destroy(InventorySystem.Instance.gameObject);
        if (MomentoSystem.Instance != null) Destroy(MomentoSystem.Instance.gameObject);
        if (LootZoneManager.Instance != null) Destroy(LootZoneManager.Instance.gameObject);
        if (TooltipManager.Instance != null) Destroy(TooltipManager.Instance.gameObject);
        if (DeathMenu.Instance != null) Destroy(DeathMenu.Instance.gameObject);
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
