using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
public class GameDirector : MonoBehaviour
{
    public static GameDirector Instance { get; private set; }



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
        InitializeDifficulty(0);
        ApplyGlobalSpawnSettings();

    }

    public void InitializeDifficulty(int baseStage)
    {
        difficultyStage = baseStage;
        gameTimer = baseStage * 30f;
        UpdateDifficultyOverTime(); // force initial difficulty sync
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

        if (gameTimer % 30f < Time.deltaTime) // Fires roughly every 30 seconds
        {
            UpdateDifficultyOverTime();
        }
    }



    private int difficultyStage = 0;

    private void UpdateDifficultyOverTime()
    {
        difficultyStage++; // increment every 30 seconds (based on Update)

        float difficultyPercent = Mathf.Clamp01((difficultyStage * 30f) / timeToMaxDifficulty);

        float newInterval = Mathf.Lerp(globalSpawnInterval, minSpawnInterval, difficultyPercent);
        int newEnemiesPerSpawn = Mathf.RoundToInt(Mathf.Lerp(baseEnemiesPerSpawn, maxEnemiesPerSpawn, difficultyPercent));

        SetGlobalSpawnInterval(newInterval);

        foreach (Spawner spawner in spawners)
        {
            spawner.SetEnemiesPerSpawn(newEnemiesPerSpawn);
        }

        Debug.Log($"[GameDirector] Difficulty Stage: {difficultyStage}, Interval: {newInterval:F2}s, Enemies: {newEnemiesPerSpawn}");
    }






    private void UpdateSpawnerStates()
    {
        if (player == null) return;

        foreach (Spawner spawner in spawners)
        {
            if (spawner == null) continue;

            float distance = Vector3.Distance(player.transform.position, spawner.transform.position);
            bool inRange = distance >= minDistanceToPlayer && distance <= maxDistanceToPlayer;

            if (inRange)
                spawner.EnableSpawning();
            else
                spawner.DisableSpawning();
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
#if UNITY_EDITOR
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
#endif
}
