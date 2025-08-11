using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_CraftableList : MonoBehaviour
{
    [SerializeField] private GameObject craftableItemSlotPrefab;
    [SerializeField] private Transform craftListParent;

    [SerializeField] private List<ItemData> weapon;
    [SerializeField] private List<ItemData> armor;
    [SerializeField] private List<ItemData> amulet;
    [SerializeField] private List<ItemData> flask;

    private void ClearAll()
    {
        foreach(Transform children in craftListParent)
        {
            Destroy(children.gameObject);
        }
    }

    public void ShowCraftableWeapon()
    {
        ClearAll();

        foreach(ItemData item in weapon)
        {
            GameObject newCraftableItem = Instantiate(craftableItemSlotPrefab, craftListParent);
            newCraftableItem.GetComponent<UI_CraftableItemSlot>().SetupSlot(new InventoryItem(item));
        }

    }

    public void ShowCraftableArmor()
    {
        ClearAll();

        foreach (ItemData item in armor)
        {
            GameObject newCraftableItem = Instantiate(craftableItemSlotPrefab, craftListParent);
            newCraftableItem.GetComponent<UI_CraftableItemSlot>().SetupSlot(new InventoryItem(item));
        }
    }

    public void ShowCraftableAmulet()
    {
        ClearAll();

        foreach (ItemData item in amulet)
        {
            GameObject newCraftableItem = Instantiate(craftableItemSlotPrefab, craftListParent);
            newCraftableItem.GetComponent<UI_CraftableItemSlot>().SetupSlot(new InventoryItem(item));
        }
    }
    public void ShowCraftableFlask()
    {
        ClearAll();

        foreach (ItemData item in flask)
        {
            GameObject newCraftableItem = Instantiate(craftableItemSlotPrefab, craftListParent);
            newCraftableItem.GetComponent<UI_CraftableItemSlot>().SetupSlot(new InventoryItem(item));
        }
    }
}
