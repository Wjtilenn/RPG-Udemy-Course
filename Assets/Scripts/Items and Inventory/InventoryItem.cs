using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InventoryItem
{
    public ItemData data;
    public int count;

    public InventoryItem(ItemData _newItemData)
    {
        data = _newItemData;
        AddStack();
    }

    public void AddStack(int _count = 1) => count += _count;
    public void RemoveStack(int _count = 1) => count = Mathf.Clamp(count - _count, 0, int.MaxValue);
}
