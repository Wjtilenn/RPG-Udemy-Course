using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class UI_EquipmentInventory : MonoBehaviour
{
    [SerializeField] private GameObject itemSlotPrefab;
    [SerializeField] private Transform itemSlotParent;
    [SerializeField] private int rowCount = 5;
    [SerializeField] private int minRowCount = 5;
    List<UI_ItemSlot> itemSlots;


    private void Awake()
    {
        itemSlots = new List<UI_ItemSlot>();
    }
    private void Start()
    {

    }

    public void UpdateItemSlotUI()
    {
        List<InventoryItem> equipmentInventory = InventoryManager.instance.GetEquipmentInventory();
        int idx = 0;
        foreach (InventoryItem item in equipmentInventory)
        {
            if (itemSlots.Count <= idx)
            {
                GameObject newItemSlot = Instantiate(itemSlotPrefab, itemSlotParent);
                itemSlots.Add(newItemSlot.GetComponent<UI_ItemSlot>());
            }
            itemSlots[idx].SetupSlot(item);
            idx++;
        }
        while (idx < itemSlots.Count) itemSlots[idx++].SetupSlot(null);

        while(itemSlots.Count < minRowCount * rowCount || itemSlots.Count % rowCount != 0)
        {
            GameObject newItemSlot = Instantiate(itemSlotPrefab, itemSlotParent);
            itemSlots.Add(newItemSlot.GetComponent<UI_ItemSlot>());
        }
    }


}
