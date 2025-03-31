using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsMenu : MonoBehaviour
{
    public static OptionsMenu Instance { get; private set; }


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }


    void OnDisable()
    {
        if (SceneManager.GetActiveScene().name != "MainMenu")
        {//if we are in the main menu, we dont need to set the game state.
            GameStateManager.Instance.SetState(GameState.Playing);
        }
    }




    public GameObject optionsMenu;
    private bool menuActivated;

    void Start()
    {
        optionsMenu.SetActive(false);
    }

    void Update()
    {
        if (Input.GetButtonDown("Options"))
        {
            menuActivated = !menuActivated;
            optionsMenu.SetActive(menuActivated);

            if (menuActivated)
            {
                if (SceneManager.GetActiveScene().name != "MainMenu")
                {
                    InventoryManager inventoryManager = FindObjectOfType<InventoryManager>();
                    inventoryManager?.CloseInventory();
                    GameStateManager.Instance.SetState(GameState.Inventory);
                }

                Debug.Log("Options Opened");
            }
            else
            {
                if (SceneManager.GetActiveScene().name != "MainMenu")
                {
                    GameStateManager.Instance.SetState(GameState.Playing);
                }

                Debug.Log("Options Closed");
            }
        }
    }

    public bool IsOptionsOpen()
    {
        return menuActivated;
    }

    public void CloseOptionsMenu()
    {
        if (menuActivated)
        {
            menuActivated = false;
            optionsMenu.SetActive(false);

            if (SceneManager.GetActiveScene().name != "MainMenu")
            {
                GameStateManager.Instance.SetState(GameState.Playing);
            }

            Debug.Log("Options Closed (Forced)");
        }
    }

}
