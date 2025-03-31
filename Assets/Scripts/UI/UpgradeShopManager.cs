using System.Collections.Generic;
using UnityEngine;

public class UpgradeShopManager : MonoBehaviour
{
    [Header("Setup")]
    public List<UpgradeData> availableUpgrades; // Drag & drop your ScriptableObjects here
    public GameObject upgradeCardPrefab; // Assign the UpgradeCardUI prefab
    public Transform cardParent; // UI container (e.g., ScrollRect > Content)

    [Header("Visuals")]
    public Sprite tickedSprite;
    public Sprite untickedSprite;

    // Simulate meta save data (you’d pull this from a save file in real use)
    private Dictionary<UpgradeData, int> upgradeLevels = new();

    void Start()
    {
        GenerateCards();
    }

    void GenerateCards()
    {
        foreach (var upgrade in availableUpgrades)
        {
            // Create a card
            GameObject cardObj = Instantiate(upgradeCardPrefab, cardParent);

            // Get and set up the card UI
            var cardUI = cardObj.GetComponent<UpgradeCardUI>();
            int currentLevel = MetaProgression.GetUpgradeLevel(upgrade.upgradeId);
            cardUI.tickedSprite = tickedSprite;
            cardUI.untickedSprite = untickedSprite;
            cardUI.Initialize(upgrade, currentLevel);

            // Optionally: hook into card to listen for upgrades and save back to your data
        }
    }


    public void RebuildCards()
    {
        foreach (Transform child in cardParent)
        {
            Destroy(child.gameObject);
        }

        GenerateCards();
    }


    int GetSavedLevelFor(UpgradeData upgrade)
    {
        // In real use, you'd query your meta-progression system or save file
        if (!upgradeLevels.ContainsKey(upgrade))
            upgradeLevels[upgrade] = 0;
        return upgradeLevels[upgrade];
    }



}
