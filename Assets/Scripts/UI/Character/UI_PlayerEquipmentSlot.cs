using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_PlayerEquipmentSlot : UI_ItemSlot
{
    public EquipmentType equipmentType;
    public override void SetupSlot(InventoryItem _newItem)
    {
        base.SetupSlot(_newItem);
    }
    public override void CleanUpSlot()
    {
        base.CleanUpSlot();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (item == null) return;

        UIManager.instance.UnequipItem(item);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
    }
}
