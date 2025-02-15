using System.Collections.Generic;
using UnityEngine;

public class GameDirector : MonoBehaviour
{
    public static GameDirector Instance { get; private set; }

    private float criticalChance = 0.05f; //default is set to 5%

    [Header("Spawner Settings")]
    public float globalSpawnInterval = 5f; //  Single reference for all spawners

    private List<Spawner> spawners = new List<Spawner>();

    private void Awake()
    {
        //  Ensure only ONE instance of GameDirector exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        FindAllSpawners();
        ApplyGlobalSpawnInterval();
    }

    /// <summary>
    ///  Finds all Spawners in the scene.
    /// </summary>
    private void FindAllSpawners()
    {
        spawners.Clear();
        GameObject[] spawnerObjects = GameObject.FindGameObjectsWithTag("Spawner");

        foreach (GameObject obj in spawnerObjects)
        {
            Spawner spawner = obj.GetComponent<Spawner>();
            if (spawner != null)
            {
                spawners.Add(spawner);
            }
        }

        Debug.Log($"GameDirector: Found {spawners.Count} spawners in the scene.");
    }

    /// <summary>
    ///  Applies the global spawn interval to all spawners.
    /// </summary>
    private void ApplyGlobalSpawnInterval()
    {
        foreach (Spawner spawner in spawners)
        {
            spawner.setIntervalTime(globalSpawnInterval);
        }
    }

    /// <summary>
    ///  Call this method if you want to change the spawn interval dynamically.
    /// </summary>
    public void SetGlobalSpawnInterval(float newInterval)
    {
        globalSpawnInterval = newInterval;
        ApplyGlobalSpawnInterval();
        Debug.Log($"GameDirector: Updated spawn interval to {globalSpawnInterval} seconds.");
    }

    public float GetCriticalChance() => criticalChance; // returns crit chance
}
