using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CraftPlant : MonoBehaviour
{
    [SerializeField] private GameObject craftingMaterialPrefab;
    [SerializeField] private Transform craftingMaterialParent;

    private ItemData_Equipment item;
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI description;


    public void SetupCraftableItem(ItemData_Equipment _item)
    {
        item = _item;

        if(_item == null)
        {
            itemImage.sprite = null;
            itemImage.color = Color.clear;
            itemName.text = "";
            description.text = "";
            ClearCraftingMaterials();
            return;
        }

        itemImage.sprite = _item.icon;
        itemImage.color = Color.white;
        itemName.text = _item.name;

        description.text = item.GetDescription();

        ShowCraftingMaterials(_item.GetCraftingMaterials());
    }

    private void ClearCraftingMaterials()
    {
        for(int i = craftingMaterialParent.transform.childCount - 1;i >= 0; i--)
        {
            Destroy(craftingMaterialParent.transform.GetChild(i).gameObject);
        }
    }
    
    public void ShowCraftingMaterials(List<InventoryItem> _craftingMaterials)
    {
        ClearCraftingMaterials();

        foreach(InventoryItem item in _craftingMaterials)
        {
            GameObject newItemSlot = Instantiate(craftingMaterialPrefab, craftingMaterialParent);
            newItemSlot.GetComponent<UI_ItemSlot>().SetupSlot(item);
        }
    }

    public void CraftItem()
    {
        if (item == null) return;
        if (InventoryManager.instance.CanCraft(item))
        {
            ShowCraftingMaterials(item.GetCraftingMaterials());
        }
        
    }

}
