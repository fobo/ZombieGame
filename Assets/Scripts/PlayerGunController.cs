using UnityEngine;

public class PlayerGunController : MonoBehaviour
{
    public Gun equippedGun; // Reference to the equipped gun

    private void Update()
    {
        if (equippedGun == null) return;

        // Handle shooting input
        if (equippedGun.weaponData.fireType == FireType.Automatic && Input.GetButton("Fire1"))
        {
            equippedGun.Shoot(); // automatic fire guns
        }
        else if (equippedGun.weaponData.fireType == FireType.SingleFire && Input.GetButtonDown("Fire1"))
        {
            equippedGun.Shoot(); // single fire guns
        }

        // Handle reload input
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(equippedGun.Reload());
        }
    }
}
