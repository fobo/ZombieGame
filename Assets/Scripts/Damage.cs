using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage
{
    public int damage;
    public bool isCritical;
    public float stoppingPower;

    public Damage(int damage, bool isCritical, float stoppingPower){
        this.damage = damage; // base damage
        this.isCritical = isCritical; // checks if critical (double damage)
        this.stoppingPower = stoppingPower; // reduces the speed of enemy
    }
}
