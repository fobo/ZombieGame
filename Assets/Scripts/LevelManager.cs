using System.Collections;
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
        //play the game
        GameStateManager.Instance.SetState(GameState.Playing);
    }

    // public void LoadNextLevel()
    // {

    //     // find all game objects with the tag "DeleteOnSceneChange", and delete them



    //     //wait until all of those objects are destroyed to continue loading the next scene


    //     // Reset difficulty if you are going to the next scene
    //     int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
    //     if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
    //     {
    //         SceneManager.LoadScene(nextSceneIndex);
    //     }
    // }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadNextLevelCoroutine());
    }

    private IEnumerator LoadNextLevelCoroutine()
    {
        // Find all game objects with the tag "DeleteOnSceneChange" and delete them
        GameObject[] objectsToDelete = GameObject.FindGameObjectsWithTag("DeleteOnSceneChange");
        foreach (GameObject obj in objectsToDelete)
        {
            Destroy(obj);
        }

        // Wait until end of frame to ensure all objects are destroyed
        yield return new WaitForEndOfFrame();

        // Reset difficulty if you are going to the next scene
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }else{
            RevisitLevel(); // replays the last level
        }
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0); // this should refer to the main menu usually.
    }

    private void IncreaseDifficulty()
    {
        if(GameDirector.Instance != null){
            //increases max enemies by 5
            GameDirector.Instance.SetGlobalMaxEnemyLimit(GameDirector.Instance.GetGlobalMaxEnemyLimit() + 5);
            GameDirector.Instance.ApplyGlobalSpawnSettings();
        }
        else{
            Debug.Log("Game director not found in the level manager.");
        }
    }

    private void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
