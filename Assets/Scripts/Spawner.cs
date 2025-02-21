using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    [SerializeField] private string poolKey;       // The key for the object pool
    [SerializeField] private float intervalTime = 5f; // How often objects spawn
    [SerializeField] private int enemiesPerSpawn = 1; // Number of enemies per spawn

    private bool canSpawn = true;
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
            SpawnGameObjects(); // Spawn multiple enemies
            currTime -= intervalTime;
        }
    }

    /// <summary>
    /// Spawns multiple enemies from the pool.
    /// </summary>
    void SpawnGameObjects()
    {
        if (!canSpawn) return;

        for (int i = 0; i < enemiesPerSpawn; i++)
        {
            SpawnSingleGameObject();
        }
    }

    /// <summary>
    /// Spawns a single enemy from the pool.
    /// </summary>
    void SpawnSingleGameObject()
    {
        if (string.IsNullOrEmpty(poolKey))
        {
            Debug.LogError($"Spawner {name}: poolKey is not set!");
            return;
        }

        GameObject obj = GameController.Instance.GetPooledObject(poolKey, transform.position, transform.rotation);

        if (obj != null)
        {
            if (obj.TryGetComponent<ISpawnable>(out var spawnable))
            {
                spawnable.OnSpawned(poolKey);
            }
        }
        else
        {
            Debug.LogError($"Spawner {name} failed to get object from pool: {poolKey}");
        }
    }

    /// <summary>
    /// Public setter to change spawn interval dynamically.
    /// </summary>
    public void setIntervalTime(float time)
    {
        intervalTime = time;
    }

    /// <summary>
    /// Public setter to change how many enemies spawn at once.
    /// </summary>
    public void SetEnemiesPerSpawn(int amount)
    {
        enemiesPerSpawn = Mathf.Max(1, amount); // Prevent zero or negative values
    }

    /// <summary>
    /// Enables the spawner.
    /// </summary>
    public void EnableSpawning()
    {
        if (!canSpawn) Debug.Log($"Spawner {name} enabled.");
        canSpawn = true;
    }

    /// <summary>
    /// Disables the spawner.
    /// </summary>
    public void DisableSpawning()
    {
        if (canSpawn) Debug.Log($"Spawner {name} disabled.");
        canSpawn = false;
    }
}
