using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SceneChanger : MonoBehaviour
{
    public void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.LogWarning("No more scenes in the build index!");
        }
    }

    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //     Debug.Log("Collided");

    //     if (other.CompareTag("Player"))
    //     {
    //         // Return all active pooled objects before loading the new scene
    //         ReturnAllPooledObjects();

    //         LoadNextScene();
    //     }
    // }

    public void ReturnAllPooledObjects()
    {
        if (GameController.Instance == null)
        {
            Debug.LogError("GameController instance not found! Cannot return pooled objects.");
            return;
        }

        // Iterate through all objects in the scene
        List<GameObject> objectsToReturn = new List<GameObject>();

        foreach (Transform obj in FindObjectsOfType<Transform>())
        {
            foreach (var poolItem in GameController.Instance.poolItems)
            {
                // Check if object was instantiated from a pooled prefab
                if (obj.name.StartsWith(poolItem.prefab.name))
                {
                    objectsToReturn.Add(obj.gameObject);
                }
            }
        }

        // Return objects to the pool
        foreach (GameObject obj in objectsToReturn)
        {
            foreach (var poolItem in GameController.Instance.poolItems)
            {
                if (obj.name.StartsWith(poolItem.prefab.name))
                {
                    GameController.Instance.ReturnToPool(poolItem.poolKey, obj);
                }
            }
        }

        Debug.Log($"Returned {objectsToReturn.Count} objects to the pool.");
    }
}
