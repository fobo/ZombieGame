using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{

    public GameObject InventoryMenu; // this is the reference to the GUI that controls the inventory.
    private bool menuActivated; // keeps track of if the inventory is open or not.
    // Start is called before the first frame update
    void Start()
    {
        InventoryMenu.SetActive(false); // Ensure menu starts hidden
    }

    void Update()
    {
        if (Input.GetButtonDown("Inventory"))
        {
            menuActivated = !menuActivated; //  Toggle state
            InventoryMenu.SetActive(menuActivated); //  Apply state
            Debug.Log(menuActivated ? "Inventory Opened" : "Inventory Closed");
        }

        if (menuActivated)
        {
            FindObjectOfType<WeaponUIManager>()?.UpdateWeaponUI();
            FindObjectOfType<ConsumablesUIManager>()?.UpdateConsumablesUI(); // Update ammo

            Time.timeScale = 0; //  Pause the game
        }
        else
        {
            Time.timeScale = 1; //  Resume the game
        }
    }
}
