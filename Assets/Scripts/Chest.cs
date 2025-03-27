using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class Chest : MonoBehaviour
{
    public Sprite openedChestSprite;  // Assign the opened chest sprite in Inspector
    public GameObject itemPrefab;     // Assign the item prefab in Inspector
    public Transform dropPosition;    // Drop position
    [SerializeField] private int treasureClass = 3; // Default tier for chests
    private bool isOpened = false;
    [SerializeField] private GameObject promptUI;


    private void Start()
    {
        TryUpgradeTreasureClass(); // Upgrade chest loot tier on load
    }

    private void TryUpgradeTreasureClass()
    {
        float upgradeChance = 0.05f;

        if (Util.RollChance(upgradeChance))
        {
            if (treasureClass == 3) treasureClass = 6;
            else if (treasureClass == 6) treasureClass = 9;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isOpened)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                Debug.LogWarning("Opening");
                OpenChest();
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isOpened)
        {
            promptUI.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            promptUI.SetActive(false);
        }
    }

    private void Update()
    {
        if (promptUI.activeSelf && Input.GetKeyDown(KeyCode.F))
        {
            OpenChest();
        }
    }
    private void OpenChest()
    {
        isOpened = true;
        promptUI.SetActive(false);

        GetComponent<SpriteRenderer>().sprite = openedChestSprite;

        if (itemPrefab != null && dropPosition != null)
        {
            GameObject mysteryItem = Instantiate(itemPrefab, dropPosition.position, Quaternion.identity);

            PrefabItemDropper prefrabItemScript = mysteryItem.GetComponent<PrefabItemDropper>();
            if (prefrabItemScript != null)
            {
                prefrabItemScript.SetTreasureClass(treasureClass);
            }
        }

        GetComponent<Collider2D>().enabled = false;
    }

    public void SetTreasureClass(int newTC)
    {
        treasureClass = newTC;
    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.yellow;
        style.fontSize = 12;
        style.alignment = TextAnchor.MiddleCenter;

        UnityEditor.Handles.Label(transform.position + Vector3.up * .5f, $"TC: {treasureClass}", style);
    }

#endif
}
