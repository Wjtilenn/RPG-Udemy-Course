using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "物品效果", menuName = "Data/Item effect")]
public class ItemEffect : ScriptableObject
{
    
    public virtual void ExecuteEffect(Transform _enemyPosition)
    {
        Debug.Log("执行效果");
    }

}
