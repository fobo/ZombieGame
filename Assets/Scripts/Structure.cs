using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// A structure type enemy. Usually this would be something like a spawner, or maybe a crate that the player can destroy.
/// Assign this script to things you want the player to attack for loot. Dont forget to assign the Die() method in the inspector when onHealthDepleted is invoked.
/// If you don't want a structure to die, don't apply this or a health component to it.
/// </summary>
public class Structure : MonoBehaviour
{


    [SerializeField] private int treasureClassLevel = 1;
    [SerializeField] private HealthComponent healthComponent;
    [SerializeField] private GameObject itemPrefab; // Assign in Inspector
    // Start is called before the first frame update
    private void Awake()
    {
        healthComponent = GetComponent<HealthComponent>();

    }

    public void Die()
    {

        //  Spawn a mystery item at the enemy's position
        if (itemPrefab != null)
        {
            GameObject mysteryItem = Instantiate(itemPrefab, transform.position, Quaternion.identity);

            //  Assign the treasure class level to the spawned item
            PrefabItemDropper prefrabItemScript = mysteryItem.GetComponent<PrefabItemDropper>();
            if (prefrabItemScript != null)
            {
                prefrabItemScript.SetTreasureClass(treasureClassLevel);
            }
        }
        else
        {
            Debug.LogWarning($"Mystery item prefab is not assigned on {gameObject.name}!");
        }

        Destroy(gameObject); // kills iteself
    }
}
