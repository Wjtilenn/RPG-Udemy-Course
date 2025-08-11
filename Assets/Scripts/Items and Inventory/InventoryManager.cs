using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class InventoryManager : MonoBehaviour, ISaveManager
{
    public static InventoryManager instance;

    public List<ItemData> startingItems;

    public List<InventoryItem> materialInventory;
    public List<InventoryItem> equipmentInventory;
    public List<InventoryItem> playerEquipment;

    [Header("物品数据库")]
    public List<ItemData> itemDataBase;
    public List<InventoryItem> loadedItems;
    public List<ItemData_Equipment> loadedEquipment;


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        materialInventory = new List<InventoryItem>();
        equipmentInventory = new List<InventoryItem>();
        playerEquipment = new List<InventoryItem>();

    }

    private void Start()
    {
        AddStartingItems();
    }

    private void AddStartingItems()
    {
        Debug.Log("加载新物品");
        for (int i = 0; i < startingItems.Count; i++)
        {
            AddItem(new InventoryItem(startingItems[i]));
        }
    }

    public void EquipItem(InventoryItem _item)
    {
        playerEquipment.Add(_item);
        RemoveItem(_item);
    }

    public void UnequipItem(InventoryItem _item)
    {
        playerEquipment.Remove(_item);
        AddItem(_item);
    }

    public void AddItem(InventoryItem _item)
    {
        if(_item.data.itemType == ItemType.Equipment)
        {
            equipmentInventory.Add(_item);
        }
        else if(_item.data.itemType == ItemType.Material)
        {
            for(int i = 0;i < materialInventory.Count;i++)
            {
                if(materialInventory[i].data == _item.data)
                {
                    materialInventory[i].AddStack(_item.count);
                    return;
                }
            }
            materialInventory.Add(_item);
        }
    }

    public void RemoveItem(InventoryItem _item, int _count = 1)
    {
        if (_item.data.itemType == ItemType.Equipment)
        {
            equipmentInventory.Remove(_item);
        }
        else if (_item.data.itemType == ItemType.Material)
        {
            for(int i =  materialInventory.Count - 1; i >= 0; i--)
            {
                if(materialInventory[i] == _item)
                {
                    materialInventory[i].RemoveStack(_count);
                    if (materialInventory[i].count <= 0)
                    {
                        materialInventory.RemoveAt(i);
                    }
                    break;
                }
            }
        }
    }

    public int GetItemCount(ItemData _item)
    {
        int count = 0;
        foreach(InventoryItem v in materialInventory)
        {
            if (v.data == _item) count += v.count;
        }
        
        return count;
    }

    public bool CanCraft(ItemData_Equipment _itemToCraft)
    {
        if (_itemToCraft == null) return false;

        List<InventoryItem> requiredMaterials = _itemToCraft.GetCraftingMaterials();
        for (int i = 0; i < requiredMaterials.Count; i++)
        {
            bool flag = false;
            for(int j = 0;j < materialInventory.Count; j++)
            {
                if (requiredMaterials[i].data == materialInventory[j].data)
                {
                    if (requiredMaterials[i].count <= materialInventory[j].count) flag = true;
                    break;
                }
            }
            if (!flag) return false;
        }
        for (int i = 0; i < requiredMaterials.Count; i++)
        {
            for (int j = 0; j < materialInventory.Count; j++)
            {
                if (requiredMaterials[i].data == materialInventory[j].data)
                {
                    
                    RemoveItem(materialInventory[j], requiredMaterials[i].count);
                    break;
                }
            }
        }


        AddItem(new InventoryItem(_itemToCraft));

        return true;
    }

    public List<InventoryItem> GetEquipmentInventory() => equipmentInventory;
    public List<InventoryItem> GetMaterialList() => materialInventory;


    public void LoadData(GameData _data)
    {
        // TODO: 如果清空背包 会重新获取初始物品

        foreach(KeyValuePair<string, int> pair in _data.inventory)
        {
            foreach(var item in itemDataBase)
            {
                if (item != null && item.itemID == pair.Key)
                {
                    InventoryItem itemToLoad = new InventoryItem(item);
                    itemToLoad.count = pair.Value;

                    loadedItems.Add(itemToLoad);
                }
            }
        }

        foreach(string loadedItemID in _data.equipmentID)
        {
            foreach(var item in itemDataBase)
            {
                if(item != null && item.itemID == loadedItemID)
                {
                    loadedEquipment.Add(item as ItemData_Equipment);
                }
            }
        }
    }

    public void SaveData(ref GameData _data)
    {
        // TODO: 保存数据
        _data.inventory.Clear();
        _data.equipmentID.Clear();

    
    }

#if UNITY_EDITOR
    [ContextMenu("更新物品数据库")]
    private void UpdateItemDataBase() => itemDataBase = new List<ItemData>(GetItemDataBase());

    private List<ItemData> GetItemDataBase()
    {
        List<ItemData> itemDataBase = new List<ItemData>();
        string[] assetNames = AssetDatabase.FindAssets("", new[] { "Assets/Data/Items" });

        foreach (string SOName in assetNames)
        {
            var SOpath = AssetDatabase.GUIDToAssetPath(SOName);
            var itemData = AssetDatabase.LoadAssetAtPath<ItemData>(SOpath);
            itemDataBase.Add(itemData);
        }

        return itemDataBase;
    }
#endif

}
