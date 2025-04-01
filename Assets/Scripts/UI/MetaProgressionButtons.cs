using System.Collections.Generic;
using UnityEngine;

public class MetaProgressionButtons : MonoBehaviour
{
    [Header("Upgrade Data Lookup (for Refunds)")]
    public UpgradeData[] allUpgrades; // Assign all UpgradeData assets in the Inspector
    public UpgradeShopManager shopManager;
    public void GiveMoney()
    {
        MetaProgression.AddCurrency(500);
    }

    public void RefundAll()
    {
        var lookup = new Dictionary<string, UpgradeData>();
        foreach (var upgrade in allUpgrades)
        {
            lookup[upgrade.upgradeId] = upgrade;
        }

        MetaProgression.RefundAllUpgrades(lookup);
        shopManager.RebuildCards(); // 🔁 Refresh cards
    }

    public void ResetAll()
    {
        MetaProgression.ResetProgress();
        shopManager.RebuildCards(); // 🔁 Refresh cards
    }
}
