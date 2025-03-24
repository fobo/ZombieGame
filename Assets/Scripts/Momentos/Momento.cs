using UnityEngine;

public abstract class Momento : MonoBehaviour
{
    public string momentoName;  // Name of the Momento
    public string description;  // Short description of the effect
    public Sprite momentoIcon;  // UI Icon

//STATS SUBJECT TO CHANGE!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    public virtual float GetDamageMultiplier() => 1f;
    public virtual float GetAPMultiplier() => 1f;
    public virtual float GetHealthMultiplier() => 1f;
    public virtual float GetFireRateMultiplier() => 1f;
    public virtual float GetReloadSpeedMultiplier() => 1f;
    public virtual float GetSpreadMultiplier() => 1f;
    public virtual float GetMoveSpeedMultiplier() => 1f;
    public virtual float GetTreasureClassMultiplier() => 1f;
    public virtual int GetLuckMultiplier() => 0;
    public virtual float GetCriticalChanceMultiplier() => 1f;
    public virtual float GetStoppingPowerMultiplier() => 1f;
    public virtual float GetCriticalDamageMultiplier() => 1f;

    /// <summary>
    /// This method is used for special effects that don't fit into normal stat adjustments.
    /// Override this in subclasses for custom effects.
    /// </summary>
    public virtual void ApplyEffect() { }
}
