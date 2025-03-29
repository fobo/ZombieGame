using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{

    private int damage;
    public float lifetime = 0.1f;
    private WeaponData weaponData;
    private float bulletapValue = 0;
    private float stoppingPower = 0;
    //AP Value for bullets. It should be assigned when it spawns and reset when it dies.
    private Gun gunScript;
    public bool isCritical = false;
    uint attackID;
    private void OnEnable()
    {
        if (weaponData != null)
        {
            bulletapValue = weaponData.apValue;// sets AP value

        }
        else
        {
            //Debug.LogWarning("Weapon data is null in Bullet!");
        }

        Invoke(nameof(ReturnToPool), lifetime);
    }

    private void OnDisable()
    {
        bulletapValue = 0; // reset the ap value to nothing by default.
        isCritical = false; // sets the critical value to false on return to the object pool
        CancelInvoke();
    }

    private void ReturnToPool()
    {
        GameController.Instance.ReturnToPool("meleeArea", gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        Debug.Log("Hitting this: " + other);
        //Debug.Log("BEFORE ap value " + bulletapValue + " for the weapon of type + " + weaponData.weaponName);
        //  Ensure the bullet only interacts with valid objects
        if (!other.CompareTag("Environment") && !other.CompareTag("Enemy") && !other.CompareTag("Structure"))
        {
            Debug.Log("compare tags");

            return;
        }

        HealthComponent health = other.GetComponent<HealthComponent>();
        MovementController movement = other.GetComponent<MovementController>();
        Enemy enemy = other.GetComponent<Enemy>();
        //        if (movement != null)
        //        {
        //            Debug.Log("movement != null");
        //            //reduce the move speed based on weapondata.stoppingPower
        //            movement.ApplyStoppingPower(weaponData.stoppingPower);
        //
        //        }
        Debug.Log("HEALTH:" + health);
        attackID = (uint) Random.Range(0, uint.MaxValue);
        if (enemy != null && enemy.hitID != attackID)
        {
            Debug.Log(enemy.hitID + " " + attackID);
            enemy.hitID = attackID; // Store attackID to prevent re-hitting
            Damage damageC = new Damage(damage, isCritical, 0f);
            health.TakeDamage(damageC);
            Debug.Log("time to hit");
        }

    }


    public void setCritical()
    {
        isCritical = true;
    }

    public bool GetCritical() => isCritical;

    public void SetWeaponData(WeaponData data)
    {
        weaponData = data;
        damage = weaponData.damage;
        stoppingPower = weaponData.stoppingPower;
        if (Util.RollChance(weaponData.criticalChance))
        {
            Debug.Log("Critical!");
            damage *= (int)(2 * MomentoSystem.Instance.GetCriticalDamageMultiplier()); // doubles the damage and adds crit damage mult
            isCritical = true;
        }

    }

}
