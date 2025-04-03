using UnityEngine;

public static class Util
{
    /// <summary>
    /// Determines if a random float between 0 and 1 meets or exceeds the target success probability. Uses luck.
    /// </summary>
    /// <param name="targetSuccess">The probability threshold (between 0 and 1).</param>
    /// <returns>True if the random value is less than or equal to the targetSuccess, otherwise false.</returns>
    public static bool RollChance(float targetSuccess)
    {
        int luckFactor = MomentoSystem.Instance.GetLuckMultiplier();

        if (targetSuccess < 0f || targetSuccess > 1f)
        {
            Debug.LogWarning("RollChance was given an out-of-bounds probability. Clamping value between 0 and 1.");
            targetSuccess = Mathf.Clamp01(targetSuccess);
        }

        // Attempt the roll up to luckFactor + 1 times (original attempt + retries)

        for (int i = 0; i <= luckFactor; i++)
        {

            if (Random.value <= targetSuccess)
            {
                return true; // Success
            }
        }

        return false; // Failure after all attempts
    }

    /// <summary>
    /// Determines if a random float between 0 and 1 meets or exceeds the target success probability. Does not use luck.
    /// </summary>
    /// <param name="targetSuccess">The probability threshold (between 0 and 1).</param>
    /// <returns>True if the random value is less than or equal to the targetSuccess, otherwise false.</returns>
    public static bool RollChanceNoLuck(float targetSuccess)
    {
        if (targetSuccess < 0f || targetSuccess > 1f)
        {
            Debug.LogWarning("RollChanceNoLuck was given an out-of-bounds probability. Clamping value between 0 and 1.");
            targetSuccess = Mathf.Clamp01(targetSuccess);
        }

        // Single roll based purely on chance
        return Random.value <= targetSuccess;
    }


    public static WeaponData updateWeaponData(WeaponData weaponData)
    {


        //On weapon equip, update all of the weapon data to be accurate based on the current values of the momento system.
        //Also please call this method whenever the player picks up a momento.


        // Apply multipliers from MomentoSystem
        weaponData.apValue *= MomentoSystem.Instance.GetAPMultiplier();
        weaponData.damage *= MomentoSystem.Instance.GetDamageMultiplier();
        weaponData.fireRate *= MomentoSystem.Instance.GetFireRateMultiplier();
        weaponData.reloadSpeed *= MomentoSystem.Instance.GetReloadSpeedMultiplier();
        weaponData.spread *= MomentoSystem.Instance.GetSpreadMultiplier();
        weaponData.criticalChance *= MomentoSystem.Instance.GetCriticalChanceMultiplier();
        weaponData.stoppingPower *= MomentoSystem.Instance.GetStoppingPowerMultiplier();

        return weaponData;
    }


    public static int GetTCForZoneTier(int tier, bool isChest)
    {
        switch (tier)
        {
            case 1:
                return isChest ? 3 : Random.Range(0, 3); // 0, 1, 2
            case 2:
                return isChest ? Random.value < 0.5f ? 3 : 6 : Random.Range(4, 6); // 4 or 5
            case 3:
            case 4:
                return isChest ? Random.Range(0, 3) * 3 + 3 : Random.Range(7, 9); // 3, 6, 9 and 7 or 8
            default:
                Debug.LogWarning($"[LootZoneTierUtil] Unknown zone tier: {tier}");
                return isChest ? 3 : 0;
        }
    }

}
