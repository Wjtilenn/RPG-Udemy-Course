using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class UI_Craft : MonoBehaviour
{
    private UI_CraftableList craftList;
    private UI_CraftPlant craftPlant;

    [SerializeField] private GameObject itemSlotPrefab;
    [SerializeField] private Transform materialInventoryParent;
    [SerializeField] private int rowCount = 9;
    [SerializeField] private int minRowCount = 3;
    List<UI_ItemSlot> itemSlots;

    private void Awake()
    {
        craftList = GetComponentInChildren<UI_CraftableList>();
        craftPlant = GetComponentInChildren<UI_CraftPlant>();   
        itemSlots = new List<UI_ItemSlot>();
    }

    private void OnEnable()
    {
        craftList.ShowCraftableWeapon();
        craftPlant.SetupCraftableItem(null);
        UpdateMaterialInventory();
    }

    public void UpdateMaterialInventory()
    {

        List<InventoryItem> materialInventory = InventoryManager.instance.GetMaterialList();
        int idx = 0;
        foreach (InventoryItem item in materialInventory)
        {
            if (itemSlots.Count <= idx)
            {
                GameObject newItemSlot = Instantiate(itemSlotPrefab, materialInventoryParent);
                itemSlots.Add(newItemSlot.GetComponent<UI_ItemSlot>());
            }
            itemSlots[idx].SetupSlot(item);
            idx++;
        }
        while (idx < itemSlots.Count) itemSlots[idx++].SetupSlot(null);

        while (itemSlots.Count < minRowCount * rowCount || itemSlots.Count % rowCount != 0)
        {
            GameObject newItemSlot = Instantiate(itemSlotPrefab, materialInventoryParent);
            itemSlots.Add(newItemSlot.GetComponent<UI_ItemSlot>());
        }
    }

    public void SetupCraftableItem(ItemData_Equipment _item)
    {
        craftPlant.SetupCraftableItem(_item);
    }
    public void CraftItem()
    {
        craftPlant.CraftItem();
        UpdateMaterialInventory();

    }
}
