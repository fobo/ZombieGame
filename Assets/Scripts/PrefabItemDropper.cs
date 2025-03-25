using UnityEngine;
using System.Collections.Generic;

public class PrefabItemDropper : MonoBehaviour
{
    [SerializeField] private int treasureClass; // Single treasure class now

    public void SetTreasureClass(int tc)
    {
        treasureClass = TryUpgradeTreasureClass(tc);
        GenerateLoot();
    }

    private int TryUpgradeTreasureClass(int baseTC)
    {
        float upgradeChance = 0.01f;

        // Linear upgrade paths
        Dictionary<int, int> primaryUpgrades = new Dictionary<int, int>
    {
        { 0, 4 }, { 4, 7 }, // Crafting
        { 1, 2 }, { 2, 5 }, { 5, 8 }, // Ammo
        { 3, 6 }, { 6, 9 }  // Momentos
    };

        // Optional conversions from Crafting to Ammo (secondary path)
        Dictionary<int, int> altUpgrades = new Dictionary<int, int>
    {
        { 0, 1 },
        { 4, 5 },
        { 7, 8 }
    };

        // Try primary upgrade path first
        if (Util.RollChance(upgradeChance) && primaryUpgrades.ContainsKey(baseTC))
        {
            return primaryUpgrades[baseTC];
        }

        // If not upgraded, try alternate upgrade path
        if (Util.RollChance(upgradeChance) && altUpgrades.ContainsKey(baseTC))
        {
            return altUpgrades[baseTC];
        }

        return baseTC;
    }


    private void GenerateLoot()
    {
        GameObject itemToSpawn = LootTableManager.Instance.GetRandomItemFromClass(treasureClass);

        if (itemToSpawn != null)
        {
            Instantiate(itemToSpawn, transform.position, Quaternion.identity);
        }

        Destroy(gameObject); // Clean up the dropper prefab
    }
}
