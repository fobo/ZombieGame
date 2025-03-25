using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public GameObject GetRandomItemFromClass(int treasureClass)

    {
        List<GameObject> combinedLootPool = new List<GameObject>();

        if (lootTable.ContainsKey(treasureClass))
        {
            List<GameObject> drops = lootTable[treasureClass];
            return drops[Random.Range(0, drops.Count)];
        }

        if (combinedLootPool.Count > 0)
        {
            int randomIndex = Random.Range(0, combinedLootPool.Count);
            return combinedLootPool[randomIndex];
        }

        //Debug.LogWarning($"No items found for Treasure Classes {minTreasureClass}-{maxTreasureClass}!");
        return null;
    }
}
