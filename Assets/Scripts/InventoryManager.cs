using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject InventoryMenu;
    private bool menuActivated;

    void Start()
    {
        InventoryMenu.SetActive(false);
    }

    void Update()
    {
        if (Input.GetButtonDown("Inventory"))
        {
            menuActivated = !menuActivated;
            InventoryMenu.SetActive(menuActivated);

            if (menuActivated)
            {
                GameStateManager.Instance.SetState(GameState.Inventory);
                FindObjectOfType<WeaponUIManager>()?.UpdateWeaponUI();
                FindObjectOfType<ConsumablesUIManager>()?.UpdateConsumablesUI();
                Debug.Log("Inventory Opened");
            }
            else
            {
                GameStateManager.Instance.SetState(GameState.Playing);

                if (TooltipManager.Instance != null)
                {
                    TooltipManager.Instance.HideTooltip();
                }
                Debug.Log("Inventory Closed");
            }
        }
    }
}
