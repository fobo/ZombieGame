using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpoiledMysteryDrink : Momento
{
    public override int GetDamageMultiplier() => 7;
    public override int GetHealthMultiplier() => -10;
}
