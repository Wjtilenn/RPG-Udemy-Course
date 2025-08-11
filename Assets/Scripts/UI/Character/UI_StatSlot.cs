using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_StatSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [TextArea]
    [SerializeField] private string statDescription;

    private void Start()
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // TODO: 状态信息提示
    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }

}
