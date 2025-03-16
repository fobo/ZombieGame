using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string tipToShow = "Testing text!";

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("entered");
        ShowMessage();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //HoverTipManager.OnMouseLoseFocus();
    }

    private void ShowMessage(){
       // HoverTipManager.OnMouseHover(tipToShow, Input.mousePosition);
    }
}
