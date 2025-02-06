using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    [Header("Object Pool Settings")]
    public List<PoolItem> poolItems; // List of all objects to be pooled

    private Dictionary<string, Queue<GameObject>> objectPools = new Dictionary<string, Queue<GameObject>>();
    private Dictionary<string, GameObject> prefabDictionary = new Dictionary<string, GameObject>();

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

        InitializePools();
    }

    /// <summary>
    /// Initializes object pools based on the `poolItems` list.
    /// </summary>
    private void InitializePools()
    {
        foreach (PoolItem item in poolItems)
        {
            if (!objectPools.ContainsKey(item.poolKey))
            {
                objectPools[item.poolKey] = new Queue<GameObject>();
                prefabDictionary[item.poolKey] = item.prefab;

                for (int i = 0; i < item.poolSize; i++)
                {
                    GameObject obj = Instantiate(item.prefab);
                    obj.SetActive(false);
                    objectPools[item.poolKey].Enqueue(obj);
                }
            }
        }
    }

    /// <summary>
    /// Retrieves an object from the pool or creates a new one if needed.
    /// </summary>
    public GameObject GetPooledObject(string poolKey, Vector3 position, Quaternion rotation)
    {
        if (objectPools.ContainsKey(poolKey) && objectPools[poolKey].Count > 0)
        {
            GameObject obj = objectPools[poolKey].Dequeue();
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            obj.SetActive(true);
            return obj;
        }

        // If the pool is empty, create a new one dynamically
        if (prefabDictionary.ContainsKey(poolKey))
        {
            Debug.LogWarning($"Pool {poolKey} is empty!");
            //return Instantiate(prefabDictionary[poolKey], position, rotation);
        }

        Debug.LogError($"Pool {poolKey} does not exist!");
        return null;
    }

    /// <summary>
    /// Returns an object to the pool for reuse.
    /// </summary>
    public void ReturnToPool(string poolKey, GameObject obj)
    {
        obj.SetActive(false);
        if (objectPools.ContainsKey(poolKey))
        {
            objectPools[poolKey].Enqueue(obj);
        }
        else
        {
            Debug.LogError($"Trying to return an object to a non-existent pool: {poolKey}");
        }
    }
}


[System.Serializable]
public struct PoolItem
{
    public string poolKey; // Unique key for each type (e.g., "Enemy", "Bullet", "RifleCasing")
    public GameObject prefab; // The prefab associated with this type
    public int poolSize; // How many should be preloaded
}
