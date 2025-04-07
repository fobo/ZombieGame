using UnityEngine;
using TMPro;

public class TutorialZone : MonoBehaviour
{
    [TextArea]
    public string tutorialMessage;

    public GameObject uiPanel;


    void Start()
    {
        if (uiPanel != null)
        {
            uiPanel.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && uiPanel != null)
        {
            uiPanel.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && uiPanel != null)
        {
            uiPanel.SetActive(false);
        }
    }
}
