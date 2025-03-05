using UnityEngine;

public class Chest : MonoBehaviour
{
    public Sprite openedChestSprite;  // Assign the opened chest sprite in Inspector
    public GameObject itemPrefab;     // Assign the item prefab in Inspector
    public Transform dropPosition;    // Drop position (assign a point slightly in front of the chest)
    [SerializeField] private int minTC = 5; // Set the loot level of the chest
    [SerializeField] private int maxTC = 10; // Set the loot level of the chest
    private bool isOpened = false;    // Track if chest has been opened

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isOpened) // Check if player is near and chest is closed
        {

            if (Input.GetKeyDown(KeyCode.F)) // Press 'F' to open
            {
                Debug.LogWarning("Opening");
                OpenChest();
            }
        }
    }

    private void OpenChest()
    {
        isOpened = true;

        // Change sprite to opened chest
        GetComponent<SpriteRenderer>().sprite = openedChestSprite;

        // Spawn item at drop position
        if (itemPrefab != null && dropPosition != null)
        {
            GameObject mysteryItem = Instantiate(itemPrefab, dropPosition.position, Quaternion.identity);

            //  Assign the treasure class level to the spawned item
            PrefabItemDropper prefrabItemScript = mysteryItem.GetComponent<PrefabItemDropper>();
            if (prefrabItemScript != null)
            {
                prefrabItemScript.SetTreasureClass(minTC, maxTC);
            }
        }

        // Disable collider so it can't be interacted with anymore
        GetComponent<Collider2D>().enabled = false;
    }
}
