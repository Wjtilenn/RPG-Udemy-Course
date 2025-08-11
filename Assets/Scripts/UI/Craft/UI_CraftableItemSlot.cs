using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_CraftableItemSlot : MonoBehaviour, IPointerDownHandler
{
    UI_ItemSlot slot;
    TextMeshProUGUI itemName;

    private void Awake()
    {
        slot = GetComponentInChildren<UI_ItemSlot>();
        itemName = transform.GetChild(1).GetComponent<TextMeshProUGUI>();   
    }

    public void SetupSlot(InventoryItem _item)
    {
        slot.SetupSlot(_item);
        itemName.text = _item.data.name;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        UIManager.instance.SetupCraftableItem(slot.item.data as ItemData_Equipment);
    }

}
