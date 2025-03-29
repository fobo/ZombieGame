using UnityEngine;

//this is an example momento. Since it inherits from the base class "Momento", we only need to
//modify what we want to change.
public class DamageBoostMomento : Momento
{
    public override int GetDamageMultiplier() => 5; //  Boost damage by 50%
}
