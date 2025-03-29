using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketDamageArea : MonoBehaviour
{
    private void Start()
    {


        Destroy(gameObject, 0.1f);
    }
    
    public Damage damageC;
    // Start is called before the first frame update
    public void SetDamage(int damage)
    {
        damageC = new Damage(damage, false, 0f);

    }


    private void OnTriggerEnter2D(Collider2D other)
    {


        HealthComponent health = other.GetComponent<HealthComponent>();

        if (health != null) //  Only apply damage if the object has health
        {
            health.TakeDamage(damageC);
        }


    }

}
