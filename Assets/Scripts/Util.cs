using UnityEngine;

public static class Util
{
    /// <summary>
    /// Determines if a random float between 0 and 1 meets or exceeds the target success probability.
    /// </summary>
    /// <param name="targetSuccess">The probability threshold (between 0 and 1).</param>
    /// <returns>True if the random value is less than or equal to the targetSuccess, otherwise false.</returns>
    public static bool RollChance(float targetSuccess)
    {
        if (targetSuccess < 0f || targetSuccess > 1f)
        {
            Debug.LogWarning("RollChance was given an out-of-bounds probability. Clamping value between 0 and 1.");
            targetSuccess = Mathf.Clamp01(targetSuccess);
        }

        return Random.value <= targetSuccess;
    }
}
