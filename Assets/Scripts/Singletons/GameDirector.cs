using System.Collections.Generic;
using UnityEngine;

public class GameDirector : MonoBehaviour
{
    public static GameDirector Instance { get; private set; }

    private float criticalChance = 0.05f; //default is set to 5%

    [Header("Spawner Settings")]
    public float globalSpawnInterval = 5f;
    public float minDistanceToPlayer = 10f;  // Too close -> disable spawner
    public float maxDistanceToPlayer = 50f;  // Too far -> disable spawner
    public float spawnerCheckInterval = 1f;  // How often to check spawner distances

    private List<Spawner> spawners = new List<Spawner>();
    private GameObject player;               // Player reference
    private float spawnerCheckTimer = 0f;    // Timer for checking spawners



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
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) Debug.LogError("Player not found!");

        FindAllSpawners();
        ApplyGlobalSpawnInterval();
    }


    private void Update()
    {
        spawnerCheckTimer += Time.deltaTime;
        if (spawnerCheckTimer >= spawnerCheckInterval)
        {
            UpdateSpawnerStates();
            spawnerCheckTimer = 0f;
        }
    }


    private void UpdateSpawnerStates()
    {
        if (player == null) return;

        foreach (Spawner spawner in spawners)
        {
            float distance = Vector3.Distance(player.transform.position, spawner.transform.position);

            if (distance < minDistanceToPlayer || distance > maxDistanceToPlayer)
            {
                spawner.DisableSpawning();
            }
            else
            {
                spawner.EnableSpawning();
            }
        }
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





    /////////gizmos
    ///


    private void OnDrawGizmos()
    {
        // Find the player if not cached
        if (player == null) player = GameObject.FindGameObjectWithTag("Player");
        if (player == null){

            
            Debug.Log("Player not found");
            return;
        } 


        if (spawners == null || spawners.Count == 0) return; // Ensure spawners list is populated

        foreach (Spawner spawner in spawners)
        {
            if (spawner == null) continue;

            float distance = Vector3.Distance(player.transform.position, spawner.transform.position);

            // Set gizmo color based on distance
            Gizmos.color = (distance >= minDistanceToPlayer && distance <= maxDistanceToPlayer) ? Color.green : Color.red;

            // Draw line and spawner sphere
            Gizmos.DrawLine(spawner.transform.position, player.transform.position);
            Gizmos.DrawSphere(spawner.transform.position, 0.2f);
        }

        // Draw player sphere
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(player.transform.position, 0.15f);
    }
}
