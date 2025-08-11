using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_CraftingMaterialSlot : UI_ItemSlot
{

    public override void SetupSlot(InventoryItem _newItem)
    {
        item = _newItem;
        itemIcon.color = Color.clear;

        if(item != null)
        {
            itemIcon.color = Color.white;
            itemIcon.sprite = item.data.icon;
            itemCount.text = InventoryManager.instance.GetItemCount(item.data).ToString() + "/" +  item.count.ToString();
        }
    }
    
    public override void CleanUpSlot()
    {
        base.CleanUpSlot();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {

    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        
    }

}
