using System.Collections.Generic;
using UnityEngine;

public class LootTableManager : MonoBehaviour
{
    public static LootTableManager Instance { get; private set; }

    [SerializeField] private List<TreasureClass> treasureClasses;

    private Dictionary<int, List<GameObject>> lootTable = new Dictionary<int, List<GameObject>>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeLootTable();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeLootTable()
    {
        for (int i = 0; i < treasureClasses.Count; i++)
        {
            lootTable[i] = treasureClasses[i].possibleDrops;
        }
    }

    public GameObject GetRandomItemFromClass(int treasureClassLevel)
    {
        List<GameObject> combinedLootPool = new List<GameObject>();

        //collect all items from TC 0 up to the requested level
        for (int i = 0; i <= treasureClassLevel; i++)
        {
            if (lootTable.ContainsKey(i))
            {
                combinedLootPool.AddRange(lootTable[i]);
            }
        }

        if (combinedLootPool.Count > 0)
        {
            int randomIndex = Random.Range(0, combinedLootPool.Count);
            return combinedLootPool[randomIndex];
        }

        Debug.LogWarning($"No items found for Treasure Class {treasureClassLevel} or below!");
        return null;
    }
}
