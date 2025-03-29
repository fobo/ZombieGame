using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogFromAPond : Momento
{
    public override int GetHealthMultiplier() => 10;
    public override float GetCriticalDamageMultiplier() => 1.1f;
}
