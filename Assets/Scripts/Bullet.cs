using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    private int damage;
    public float lifetime = 3f;
    private WeaponData weaponData;
    private int bulletapValue = 0;
    //AP Value for bullets. It should be assigned when it spawns and reset when it dies.
    private Gun gunScript;

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
        damage = weaponData.damage;
    }

    private void OnEnable()
    {
        if (weaponData != null)
        {
            bulletapValue = weaponData.apValue;
        }
        else
        {
            Debug.LogWarning("Weapon data is null in Bullet!");
        }

        Invoke(nameof(ReturnToPool), lifetime);
    }

    private void OnDisable()
    {
        bulletapValue = 0; // reset the ap value to nothing by default.
        CancelInvoke();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //  Ensure the bullet only interacts with valid objects
        if (!other.CompareTag("Environment") && !other.CompareTag("Enemy") && !other.CompareTag("Structure"))
        {
            return;
        }

        HealthComponent health = other.GetComponent<HealthComponent>();

        if (health != null) //  Only apply damage if the object has health
        {
            health.TakeDamage(damage, gameObject);
            bulletapValue -= health.GetArmorValue(); //  Only subtract armor if health exists
        }

        // ✅ Return bullet to pool if AP is 0 OR if it hits an environment object
        if (bulletapValue <= 0 || other.CompareTag("Environment"))
        {
            ReturnToPool();
        }
    }


    private void ReturnToPool()
    {
        GameController.Instance.ReturnToPool("Bullet", gameObject);
    }
}
