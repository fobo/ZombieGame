using UnityEngine;
using UnityEngine.SceneManagement;

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
                // Close Options Menu if it's open
                if (SceneManager.GetActiveScene().name != "MainMenu")
                {
                    OptionsMenu optionsMenu = FindObjectOfType<OptionsMenu>();
                    if (optionsMenu != null && optionsMenu.IsOptionsOpen())
                    {
                        optionsMenu.CloseOptionsMenu();
                    }

                    GameStateManager.Instance.SetState(GameState.Inventory);
                }

                FindObjectOfType<WeaponUIManager>()?.UpdateWeaponUI();
                FindObjectOfType<ConsumablesUIManager>()?.UpdateConsumablesUI();
                Debug.Log("Inventory Opened");
            }
            else
            {
                if (SceneManager.GetActiveScene().name != "MainMenu")
                {
                    GameStateManager.Instance.SetState(GameState.Playing);
                }

                TooltipManager.Instance?.HideTooltip();
                Debug.Log("Inventory Closed");
            }
        }
    }

    public void CloseInventory()
    {
        if (menuActivated)
        {
            menuActivated = false;
            InventoryMenu.SetActive(false);
            GameStateManager.Instance.SetState(GameState.Playing);
            TooltipManager.Instance?.HideTooltip();
            Debug.Log("Inventory Closed (Forced)");
        }
    }
}
