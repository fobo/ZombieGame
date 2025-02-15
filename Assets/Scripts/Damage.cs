using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage
{
    public float damage;
    public bool isCritical;

    public Damage(float damage, bool isCritical){
        this.damage = damage;
        this.isCritical = isCritical;
    }
}
