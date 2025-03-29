using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreshWater : Momento
{
    public override int GetDamageMultiplier() => 2;
    public override float GetFireRateMultiplier() => 1.1f;
}
