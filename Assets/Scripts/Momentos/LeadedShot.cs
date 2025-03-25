using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeadedShot : Momento
{
    public override float GetDamageMultiplier() => 1.2f;
    public override float GetStoppingPowerMultiplier() => 1.3f;
}
