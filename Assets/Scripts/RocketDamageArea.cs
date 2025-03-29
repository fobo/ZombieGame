using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketDamageArea : MonoBehaviour
{
    private void Start() {
        Destroy(gameObject, 0.1f);
    }
    bool hasAppliedDamage = false;
    public Damage damageC;
    // Start is called before the first frame update
    public void SetDamage(int damage){
        damageC = new Damage(damage, false, 0f);
        
    }


        private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("Hitting this: " + other);
        //Debug.Log("BEFORE ap value " + bulletapValue + " for the weapon of type + " + weaponData.weaponName);
        //  Ensure the bullet only interacts with valid objects


        HealthComponent health = other.GetComponent<HealthComponent>();

        if (health != null) //  Only apply damage if the object has health
        {
            health.TakeDamage(damageC);
        }


    }

}
