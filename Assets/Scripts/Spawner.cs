using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private string poolKey; // The key for the object pool
    [SerializeField] private float intervalTime = 5f; // How often objects spawn
    private float currTime;

    void Start()
    {
        currTime = 0f;
    }

    void Update()
    {
        currTime += Time.deltaTime;
        if (currTime >= intervalTime)
        {
            SpawnGameObject();
            currTime -= intervalTime; // Reset the timer
        }
    }

    /// <summary>
    /// Spawns an object from the pool and initializes it if needed.
    /// </summary>
    void SpawnGameObject()
    {
        if (!string.IsNullOrEmpty(poolKey))
        {
            GameObject obj = GameController.Instance.GetPooledObject(poolKey, transform.position, transform.rotation);

            if (obj != null)
            {
                ISpawnable spawnable = obj.GetComponent<ISpawnable>();
                if (spawnable != null)
                {
                    spawnable.OnSpawned(poolKey);
                }
            }
            else
            {
                Debug.LogError($"Spawner failed to get object from pool: {poolKey}");
            }
        }
        else
        {
            Debug.LogError("Spawner poolKey is not set!");
        }
    }
}
