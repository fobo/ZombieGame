using UnityEngine;
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
            panelToHide.SetActive(false);
        }
        else
        {
            Debug.LogWarning("No panel assigned to ReturnButton.");
        }
    }
}
