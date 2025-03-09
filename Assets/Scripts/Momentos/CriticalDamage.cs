using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriticalDamage : Momento
{
    public override float GetCriticalDamageMultiplier() => 5f; // multiplies crit damage by 5x

}
