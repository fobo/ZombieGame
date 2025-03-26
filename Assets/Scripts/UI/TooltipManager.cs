using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager Instance { get; private set; }
    public GameObject tooltipPanel;
    public TMP_Text tooltipText;
    public Vector3 offset = new Vector3(0f, -50f, 0f); // Offset to place it below the slot
    public bool isTooltipActive;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (tooltipPanel == null)
        {
            tooltipPanel = gameObject;
        }

        if (tooltipText == null)
        {
            tooltipText = GetComponentInChildren<TMP_Text>();
        }

        tooltipPanel.SetActive(false);
    }

    public void ShowTooltip(string text, Transform slotTransform)
    {
        tooltipText.text = text;

        // Position tooltip below the weapon slot, centered
        RectTransform tooltipRect = tooltipPanel.GetComponent<RectTransform>();
        RectTransform slotRect = slotTransform.GetComponent<RectTransform>();

        tooltipPanel.transform.position = new Vector3(
            slotRect.position.x, 
            slotRect.position.y + offset.y, 
            slotRect.position.z
        );

        tooltipPanel.SetActive(true);
        tooltipPanel.transform.SetAsLastSibling();

        isTooltipActive = true;
    }

    public void HideTooltip()
    {
        tooltipPanel.SetActive(false);
        isTooltipActive = false;
    }
}
