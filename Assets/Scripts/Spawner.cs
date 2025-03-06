using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    [SerializeField] private GameObject enemyPrefab; // The enemy prefab to spawn
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
    /// Spawns multiple enemies by instantiating them.
    /// </summary>
    void SpawnGameObjects()
    {
        if (!canSpawn || enemyPrefab == null) return;

        for (int i = 0; i < enemiesPerSpawn; i++)
        {
            SpawnSingleGameObject();
        }
    }

    /// <summary>
    /// Spawns a single enemy by instantiating a new GameObject.
    /// </summary>
    void SpawnSingleGameObject()
    {
        if (enemyPrefab == null)
        {
            Debug.LogError($"Spawner {name}: Enemy prefab is not assigned!");
            return;
        }

        Instantiate(enemyPrefab, transform.position, transform.rotation);
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

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red; // Set Gizmo color
        Gizmos.DrawSphere(transform.position, 0.5f); // Draw a sphere at the spawn point

        // Draw text label above the spawn point
        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.white;
        style.fontSize = 14;
        style.alignment = TextAnchor.MiddleCenter;

        Handles.Label(transform.position + Vector3.up * 0.75f, "Enemy Spawner", style);
    }
#endif
}
