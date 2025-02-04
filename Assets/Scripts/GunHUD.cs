using UnityEngine;

public class GunHUD : MonoBehaviour
{
    public Animator HUDShellAnimation; // Prefab for shell animation, MAKE SURE THIS IS AN ANIMATION!!
    public Transform shellSpawnPoint; // Where the shell should appear in the HUD, Should normally be 0,0,0
    public Transform weaponHUDParent; // Reference to the HUD (parent object in Virtual Camera)

public void PlayShellEjectionAnimation()
{
    if (HUDShellAnimation != null)
    {
        // Instantiate the shell animation under the Weapon HUD
        Animator shellInstance = Instantiate(HUDShellAnimation, weaponHUDParent);

        // Reset local position so it appears at (0,0,0) inside the HUD
        shellInstance.transform.localPosition = Vector3.zero;
        shellInstance.transform.localRotation = Quaternion.identity;

        //  Force the animation to play
        shellInstance.Play("AK_Shell_Casing"); // Replace with your actual animation name

        // Destroy the animation after it plays
        //Destroy(shellInstance.gameObject, 1f);
    }
}

}
