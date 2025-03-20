using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections.Generic;

public class MomentoUIManager : MonoBehaviour
{
    public GameObject momentoSlotPrefab;
    public Transform momentoPanel;
    public ScrollRect scrollRect;

    private void OnEnable()
    {
        UpdateMomentoUI();
    }

    public void UpdateMomentoUI()
    {
        foreach (Transform child in momentoPanel)
        {
            Destroy(child.gameObject);
        }

        List<Momento> momentos = InventorySystem.Instance.GetMomentos();

        foreach (Momento momento in momentos)
        {
            GameObject newMomentoSlot = Instantiate(momentoSlotPrefab, momentoPanel);

            newMomentoSlot.transform.Find("MomentoIcon").GetComponent<Image>().sprite = momento.momentoIcon;
           // newMomentoSlot.transform.Find("MomentoNameText").GetComponent<TMP_Text>().text = momento.momentoName;

            AddTooltipTriggers(newMomentoSlot, momento);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(momentoPanel.GetComponent<RectTransform>());
    }

    private void Update()
    {
        if (IsMouseOverUI(momentoPanel))
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            scrollRect.verticalNormalizedPosition += scroll;
        }
    }

    private bool IsMouseOverUI(Transform panel)
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.transform.IsChildOf(panel))
            {
                return true;
            }
        }
        return false;
    }

    private void AddTooltipTriggers(GameObject momentoSlot, Momento momento)
    {
        EventTrigger trigger = momentoSlot.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = momentoSlot.AddComponent<EventTrigger>();
        }
        trigger.triggers.Clear();

        string tooltipText = $"{momento.momentoName}\n{momento.description}";

        EventTrigger.Entry entryEnter = new EventTrigger.Entry();
        entryEnter.eventID = EventTriggerType.PointerEnter;
        entryEnter.callback.AddListener((eventData) =>
        {
            if (TooltipManager.Instance != null)
            {
                TooltipManager.Instance.ShowTooltip(tooltipText, momentoSlot.transform);
            }
        });
        trigger.triggers.Add(entryEnter);

        EventTrigger.Entry entryExit = new EventTrigger.Entry();
        entryExit.eventID = EventTriggerType.PointerExit;
        entryExit.callback.AddListener((eventData) =>
        {
            if (TooltipManager.Instance != null)
            {
                TooltipManager.Instance.HideTooltip();
            }
        });
        trigger.triggers.Add(entryExit);
    }
}
