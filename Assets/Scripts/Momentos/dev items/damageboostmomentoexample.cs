using UnityEngine;

//this is an example momento. Since it inherits from the base class "Momento", we only need to
//modify what we want to change.
public class DamageBoostMomento : Momento
{
    public override float GetDamageMultiplier() => 1.5f; //  Boost damage by 50%
}
