using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    private float damage;
    public float lifetime = 3f;
    private WeaponData weaponData;
    private float bulletapValue = 0;
    //AP Value for bullets. It should be assigned when it spawns and reset when it dies.
    private Gun gunScript;
    public bool isCritical = false;

    void Start()
    {

    }
    private void Update()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);
    }


    public void SetWeaponData(WeaponData data)
    {
        weaponData = data;
        bulletapValue = weaponData.apValue; // weaponData is assigned before use
        damage = weaponData.damage * MomentoSystem.Instance.GetDamageMultiplier();

        if(isCritical){
            damage *= 2; // doubles the damage
        }

    } 

    private void OnEnable()
    {
        if (weaponData != null)
        {
            bulletapValue = weaponData.apValue;

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

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("Hitting this: " + other);
        //Debug.Log("BEFORE ap value " + bulletapValue + " for the weapon of type + " + weaponData.weaponName);
        //  Ensure the bullet only interacts with valid objects
        if (!other.CompareTag("Environment") && !other.CompareTag("Enemy") && !other.CompareTag("Structure"))
        {
            return;
        }

        HealthComponent health = other.GetComponent<HealthComponent>();

        if (health != null) //  Only apply damage if the object has health
        {
            Damage damageC = new Damage(damage, isCritical);
            health.TakeDamage(damageC);
            //Debug.Log(damage);
            //Debug.Log(damageC.damage);
            bulletapValue -= health.GetArmorValue(); //  Only subtract armor if health exists
        }

        //  Return bullet to pool if AP is 0 OR if it hits an environment object
        if (bulletapValue <= 0 || other.CompareTag("Environment"))
        {
            ReturnToPool();
        }
        //Debug.Log("AFTER ap value " + bulletapValue + " for the weapon of type + " + weaponData.weaponName);
    }


    public void setCritical(){
        isCritical = true;
    }

    public bool GetCritical() => isCritical;
    private void ReturnToPool()
    {
        GameController.Instance.ReturnToPool("Bullet", gameObject);
    }
}
