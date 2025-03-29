using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeadedShot : Momento
{
    public override int GetDamageMultiplier() => 2;
    public override float GetStoppingPowerMultiplier() => 1.3f;
}
