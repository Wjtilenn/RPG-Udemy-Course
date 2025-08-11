using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UI_ItemSlot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    protected Image itemIcon;
    protected TextMeshProUGUI itemCount;

    public InventoryItem item;

    protected virtual void Awake()
    {
        foreach(Transform child in transform)
        {
            itemIcon = child.GetComponent<Image>();
            if (itemIcon != null) break;
        }
        itemCount = GetComponentInChildren<TextMeshProUGUI>();
        item = null;
    }

    public virtual void SetupSlot(InventoryItem _newItem)
    {
        item = _newItem;
        itemIcon.color = Color.clear;
        itemCount.text = "";
        if (item != null)
        {
            itemIcon.color = Color.white;
            itemIcon.sprite = item.data.icon;
            if (item.count > 1)
            {
                itemCount.text = item.count.ToString();
            }
        }
    }

    public virtual void CleanUpSlot()
    {
        item = null;
        itemIcon.sprite = null;
        itemIcon.color = Color.clear;
        itemCount.text = "";
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (item == null) return;

        if (item.data.itemType == ItemType.Equipment)
        {
            UIManager.instance.EquipItem(item);
        }

    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (item == null) return;
        // TODO: ŒÔ∆∑√Ë ˆ
        //ui.itemToolTip.ShowToolTip(item.data as ItemData_Equipment);
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        if(item == null) return;

        //ui.itemToolTip.HideToolTip();
    }
}
