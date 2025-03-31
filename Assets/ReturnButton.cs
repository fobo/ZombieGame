using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ReturnButton : MonoBehaviour
{
    [Tooltip("The panel to hide when this button is clicked.")]
    public GameObject panelToHide;

    private void Start()
    {
        Button btn = GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(HidePanel);
        }
        else
        {
            Debug.LogWarning("ReturnButton script is missing a Button component!");
        }
    }

    private void HidePanel()
    {
        if (panelToHide != null)
        {

            if (SceneManager.GetActiveScene().name != "MainMenu")
            {//if we are in the main menu, we dont need to set the game state.
                GameStateManager.Instance.SetState(GameState.Playing);
            }
            panelToHide.SetActive(false);
        }
        else
        {
            Debug.LogWarning("No panel assigned to ReturnButton.");
        }
    }
}
