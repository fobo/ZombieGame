using UnityEngine;

public class LootZonePlaceholder : MonoBehaviour
{
    public string shapeType;
    public int tier = 1;

    private void Start()
    {
        int upgradedTier = TryUpgradeTier(tier);
        GameObject prefabToSpawn = LootZoneManager.Instance.GetRandomPrefabForZone(shapeType, upgradedTier);
        if (prefabToSpawn != null)
        {
            Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
        }
    }

    private int TryUpgradeTier(int currentTier)
    {
        //implement this later
        return currentTier;
    }
}
