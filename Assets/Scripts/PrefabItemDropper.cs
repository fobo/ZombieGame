using UnityEngine;

public class PrefabItemDropper : MonoBehaviour
{
    [SerializeField] private int treasureClassLevel; // Assign a TC level when spawning



    public void SetTreasureClass(int level)
    {
        treasureClassLevel = level;

        GenerateLoot();// when this prefab is ready, it just generates the loot, then it KILLS itself sigma skibidy
    }

    private void GenerateLoot()
    {
        GameObject itemToSpawn = LootTableManager.Instance.GetRandomItemFromClass(treasureClassLevel);

        if (itemToSpawn != null)
        {
            Debug.Log("generating " + itemToSpawn);
            Instantiate(itemToSpawn, transform.position, Quaternion.identity);
        }

        Destroy(gameObject); // Remove the mystery item after spawning loot
    }

}
