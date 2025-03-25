using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreshWater : Momento
{
    public override float GetDamageMultiplier() => 1.2f;
    public override float GetFireRateMultiplier() => 1.1f;
}
