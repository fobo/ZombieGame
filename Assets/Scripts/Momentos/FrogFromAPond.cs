using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogFromAPond : Momento
{
    public override float GetHealthMultiplier() => 1.1f;
    public override float GetCriticalDamageMultiplier() => 1.1f;
}
