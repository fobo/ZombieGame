using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
public class GameDirector : MonoBehaviour
{
    public static GameDirector Instance { get; private set; }


    private float criticalChance = 0.05f; //default is set to 5%
    [Header("Spawner Settings")]
    public float globalSpawnInterval = 5f;
    public float minDistanceToPlayer = 10f;
    public float maxDistanceToPlayer = 50f;
    public float spawnerCheckInterval = 1f;

    [Header("Difficulty Scaling")]
    [SerializeField] public float timeToMaxDifficulty = 300f; // 5 minutes to reach max difficulty
    [SerializeField] public float minSpawnInterval = 1f;      // Minimum allowed interval between spawns
    [SerializeField] public int baseEnemiesPerSpawn = 1;      // Enemies spawned at the start
    [SerializeField] public int maxEnemiesPerSpawn = 5;       // Max enemies spawned at once

    private List<Spawner> spawners = new List<Spawner>();
    private GameObject player;
    private float spawnerCheckTimer = 0f;
    private float gameTimer = 0f;

    private void Awake()
    {
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
        ApplyGlobalSpawnSettings();
    }

    private void Update()
    {
        gameTimer += Time.deltaTime;
        spawnerCheckTimer += Time.deltaTime;

        if (spawnerCheckTimer >= spawnerCheckInterval)
        {
            UpdateSpawnerStates();
            spawnerCheckTimer = 0f;
        }

        UpdateDifficultyOverTime();
    }

    public float GetCriticalChance() => criticalChance * MomentoSystem.Instance.GetCriticalChanceMultiplier(); // returns crit chance
    private void UpdateDifficultyOverTime()
    {
        // Calculate difficulty percentage based on elapsed time
        float difficultyPercent = Mathf.Clamp01(gameTimer / timeToMaxDifficulty);

        // Adjust spawn interval (higher difficulty = faster spawns)
        float newInterval = Mathf.Lerp(globalSpawnInterval, minSpawnInterval, difficultyPercent);
        SetGlobalSpawnInterval(newInterval);

        // Adjust the number of enemies per spawn
        int newEnemiesPerSpawn = Mathf.RoundToInt(Mathf.Lerp(baseEnemiesPerSpawn, maxEnemiesPerSpawn, difficultyPercent));
        foreach (Spawner spawner in spawners)
        {
            spawner.SetEnemiesPerSpawn(newEnemiesPerSpawn);
        }
    }

    private void UpdateSpawnerStates()
    {
        if (player == null) return;

        foreach (Spawner spawner in spawners)
        {
            if(spawner == null) return; // null check on potentially destroyed spawners
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

    private void FindAllSpawners()
    {
        spawners.Clear();
        GameObject[] spawnerObjects = GameObject.FindGameObjectsWithTag("Spawner");

        foreach (GameObject obj in spawnerObjects)
        {
            Spawner spawner = obj.GetComponent<Spawner>();
            if (spawner != null) spawners.Add(spawner);
        }

        Debug.Log($"GameDirector: Found {spawners.Count} spawners.");
    }

    private void ApplyGlobalSpawnSettings()
    {
        foreach (Spawner spawner in spawners)
        {
            spawner.setIntervalTime(globalSpawnInterval);
            spawner.SetEnemiesPerSpawn(baseEnemiesPerSpawn);
        }
    }

    public void SetGlobalSpawnInterval(float newInterval)
    {
        globalSpawnInterval = newInterval;
        foreach (Spawner spawner in spawners)
        {
            spawner.setIntervalTime(globalSpawnInterval);
        }

        //Debug.Log($"GameDirector: Updated spawn interval to {globalSpawnInterval:F2} seconds.");
    }

    private void OnDrawGizmos()
    {
        if (player == null) player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        if (spawners == null || spawners.Count == 0) return;

        foreach (Spawner spawner in spawners)
        {
            if (spawner == null) continue;

            float distance = Vector3.Distance(player.transform.position, spawner.transform.position);
            Gizmos.color = (distance >= minDistanceToPlayer && distance <= maxDistanceToPlayer) ? Color.green : Color.red;

            Vector3 midPoint = (spawner.transform.position + player.transform.position) / 2;

            Gizmos.DrawLine(spawner.transform.position, player.transform.position);
            Gizmos.DrawSphere(spawner.transform.position, 0.2f);

            // Draw the distance label at the midpoint of the line
            Handles.Label(midPoint, distance.ToString("F2") + "m");
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(player.transform.position, 0.15f);
    }
}
