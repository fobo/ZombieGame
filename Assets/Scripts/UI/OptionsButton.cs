using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsButton : MonoBehaviour
{
    [Tooltip("The panel to show when this button is clicked.")]
    public GameObject panelToShow;

    private void Start()
    {
        Button btn = GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(ShowPanel);
        }
        else
        {
            Debug.LogWarning("options button script is missing a Button component!");
        }
    }

    private void ShowPanel()
    {
        if (panelToShow != null)
        {
            panelToShow.SetActive(true);
        }
        else
        {
            Debug.LogWarning("No panel assigned to options button.");
        }
    }


    
}
