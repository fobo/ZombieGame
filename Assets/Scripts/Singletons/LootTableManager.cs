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

    public GameObject GetRandomItemFromClass(int minTreasureClass, int maxTreasureClass)
    {
        List<GameObject> combinedLootPool = new List<GameObject>();

        // Ensure min is at least 0 and max does not exceed available classes
        minTreasureClass = Mathf.Max(minTreasureClass, 0);
        maxTreasureClass = Mathf.Min(maxTreasureClass, treasureClasses.Count - 1);

        // Collect loot only within the given range
        for (int i = minTreasureClass; i <= maxTreasureClass; i++)
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

        Debug.LogWarning($"No items found for Treasure Classes {minTreasureClass}-{maxTreasureClass}!");
        return null;
    }
}
