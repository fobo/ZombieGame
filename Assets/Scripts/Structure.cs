using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A structure type enemy. Usually a spawner or breakable crate. Dies when its health is depleted.
/// </summary>
public class Structure : MonoBehaviour
{
    [SerializeField] private int TC = 0;
    [SerializeField] private HealthComponent healthComponent;
    [SerializeField] private GameObject itemPrefab;

    private void Awake()
    {
        healthComponent = GetComponent<HealthComponent>();
        TryUpgradeTreasureClass(); // Upgrade structure loot chance on spawn
    }

    private void TryUpgradeTreasureClass()
    {
        float upgradeChance = 0.01f;

        Dictionary<int, int> upgradePath = new Dictionary<int, int>
        {
            { 0, 1 }, { 1, 2 }, { 2, 4 }, { 4, 5 }, { 5, 7 }, { 7, 8 }
        };

        if (Util.RollChance(upgradeChance) && upgradePath.ContainsKey(TC))
        {
            TC = upgradePath[TC];
        }
    }

    public void Die()
    {
        if (itemPrefab != null)
        {
            GameObject mysteryItem = Instantiate(itemPrefab, transform.position, Quaternion.identity);

            PrefabItemDropper prefrabItemScript = mysteryItem.GetComponent<PrefabItemDropper>();
            if (prefrabItemScript != null)
            {
                prefrabItemScript.SetTreasureClass(TC);
            }
        }
        else
        {
            Debug.LogWarning($"Mystery item prefab is not assigned on {gameObject.name}!");
        }

        Destroy(gameObject);
    }


    public void SetTreasureClass(int newTC)
    {
        TC = newTC;
    }

    private void OnDrawGizmos()
    {
        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.cyan;
        style.fontSize = 12;
        style.alignment = TextAnchor.MiddleCenter;

        UnityEditor.Handles.Label(transform.position + Vector3.up * .5f, $"TC: {TC}", style);
    }

}
