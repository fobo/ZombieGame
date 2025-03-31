using System;
using System.Collections.Generic;

public static class MetaProgression
{
    private static int totalCurrency = 0;

    // Key: Upgrade ID, Value: Level
    public static Dictionary<string, int> upgradeLevels = new();


    //actions
    public static event Action OnCurrencyChanged;


    /// <summary>Resets all currency and upgrade levels to zero.</summary>
    public static void ResetProgress()
    {
        totalCurrency = 0;
        upgradeLevels.Clear();
        OnCurrencyChanged?.Invoke();
    }

    /// <summary>Refunds all upgrades into currency and clears upgrad
    // e levels.</summary>
    public static void RefundAllUpgrades(Dictionary<string, UpgradeData> upgradeDataById)
    {
        int refund = 0;

        foreach (var pair in upgradeLevels)
        {
            string id = pair.Key;
            int level = pair.Value;

            if (upgradeDataById.TryGetValue(id, out var data))
            {
                // Calculate total cost paid for this upgrade
                for (int i = 1; i <= level; i++)
                {
                    refund += data.baseCost * i;
                }
            }
        }

        totalCurrency += refund;
        upgradeLevels.Clear();
        OnCurrencyChanged?.Invoke();
    }

    /// <summary>Attempts to spend currency. Returns true if successful.</summary>
    public static bool SpendCurrency(int amount)
    {
        if (totalCurrency < amount)
            return false;

        totalCurrency -= amount;
        OnCurrencyChanged?.Invoke();
        return true;
    }

    public static void AddCurrency(int amount)
    {
        totalCurrency += amount;
        OnCurrencyChanged?.Invoke();
    }

    public static int GetCurrency()
    {
        return totalCurrency;
    }

    /// <summary>Returns the current level for the given upgrade.</summary>
    public static int GetUpgradeLevel(string upgradeId)
    {
        return upgradeLevels.TryGetValue(upgradeId, out var level) ? level : 0;
    }

    /// <summary>Sets the level for an upgrade.</summary>
    public static void SetUpgradeLevel(string upgradeId, int level)
    {
        upgradeLevels[upgradeId] = level;
    }
}
