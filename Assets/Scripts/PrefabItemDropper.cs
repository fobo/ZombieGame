using UnityEngine;

public class PrefabItemDropper : MonoBehaviour
{
    [SerializeField] private int minTreasureClass; // Assign a TC level when spawning
    [SerializeField] private int maxTreasureClass; // Assign a TC level when spawning


    public void SetTreasureClass(int minLevel, int maxLevel)
    {
        minTreasureClass = minLevel;
        maxTreasureClass = maxLevel;

        GenerateLoot();// when this prefab is ready, it just generates the loot, then it KILLS itself sigma skibidy
    }

    private void GenerateLoot()
    {
        GameObject itemToSpawn = LootTableManager.Instance.GetRandomItemFromClass(minTreasureClass, maxTreasureClass);

        if (itemToSpawn != null)
        {
           // Debug.Log("generating " + itemToSpawn);
            Instantiate(itemToSpawn, transform.position, Quaternion.identity);
        }

        Destroy(gameObject); // Remove the mystery item after spawning loot
    }

}
