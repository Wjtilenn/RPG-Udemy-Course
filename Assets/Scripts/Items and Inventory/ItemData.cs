using System.Text;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public enum ItemType
{
    Material,
    Equipment
}

[CreateAssetMenu(fileName = "物品数据", menuName = "Data/Item")]
public class ItemData : ScriptableObject
{
    public string itemID;

    public ItemType itemType;
    public string itemName;
    public Sprite icon;

    [TextArea]
    [SerializeField] protected string itemDescription;

    private void OnValidate()
    {
#if UNITY_EDITOR

        string path = AssetDatabase.GetAssetPath(this);
        itemID = AssetDatabase.AssetPathToGUID(path);
#endif  
    }

    public virtual string GetDescription()
    {
        return itemDescription.ToString();
    }

}
