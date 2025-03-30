using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    public float speed = 1f;
    private int damage;
    public float lifetime = 10f;
    private WeaponData weaponData;
    //AP Value for bullets. It should be assigned when it spawns and reset when it dies.
    private Gun gunScript;
    public bool isCritical = false;
    private int timerForSmokeSpawn = 0;
    public GameObject smokeSpawnPos;
    public GameObject smokeAnim;
    public GameObject explosionPrefab;
    public AudioClip explosionSFX;


    private void Update()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);
    }


    public void SetWeaponData(WeaponData data)
    {
        weaponData = data;

        damage = weaponData.damage;

        if (Util.RollChance(weaponData.criticalChance))
        {
            damage *= (int)(2 * MomentoSystem.Instance.GetCriticalDamageMultiplier()); // doubles the damage and adds crit damage mult
            isCritical = true;
        }

    }

    private void FixedUpdate()
    {
        //instantiate smoke trail objects behind the rocket
        timerForSmokeSpawn += 1;// increment smoke spawn by 1
        if (timerForSmokeSpawn % 2 == 0)
        { //make smoke spawn 
            GameObject smoke = Instantiate(smokeAnim, smokeSpawnPos.transform.position, Quaternion.identity);
            timerForSmokeSpawn = 0; // reset value
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Environment") && !other.CompareTag("Enemy") && !other.CompareTag("Structure"))
        {
            return;
        }
        //blow up the rocket
        SFXManager.Instance.PlaySFXClip(explosionSFX, gameObject.transform, .2f);
        GameObject explosion = Instantiate(explosionPrefab, gameObject.transform.position, Quaternion.identity);
        RocketDamageArea bs = explosion.GetComponent<RocketDamageArea>();
        bs.SetDamage(damage);
        //create a new circle object that has a hitbox to flash damage on enemies
        Destroy(gameObject);

    }


    public void setCritical()
    {
        isCritical = true;
    }

    public bool GetCritical() => isCritical;

}
