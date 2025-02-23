using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public GameObject player;
    private void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); }
    }

    public void RevisitLevel()
    {
        player = GameObject.FindGameObjectWithTag("Player"); //we get the player here, because we know it will be in the scene
        PlayerController playerController = player.GetComponent<PlayerController>(); // get the player controller script
        IncreaseDifficulty();
        if (player != null)
        {
            SceneChanger sceneChanger = GetComponent<SceneChanger>();
            sceneChanger.ReturnAllPooledObjects(); // move all of the stuff back to the pool
            playerController.MoveToSpawnPoint(); // move the player to the initial start point of the level.
        }
        else
        {
            Debug.Log("Player controller script is null");
        }


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

    private void IncreaseDifficulty()
    {
        Debug.Log("Increasing difficulty");
    }

    private void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
