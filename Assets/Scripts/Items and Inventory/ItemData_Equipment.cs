using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.VFX;

public enum EquipmentType
{
    Weapon,     // 武器
    Armor,      // 护甲
    LegArmor,   // 护腿
    Amulet,     // 护符
}

[CreateAssetMenu(fileName = "装备数据", menuName = "Data/Equipment")]
public class ItemData_Equipment : ItemData
{
    public EquipmentType equipmentType;

    public ItemEffect[] itemEffects;

    #region 角色属性
    [Header("生命值")]
    public IntStat health;
    public FloatStat healthBouns;
    [Header("攻击力")]
    public IntStat attack;
    public FloatStat attackBouns;
    [Header("防御力")]
    public IntStat defence;
    public FloatStat defenceBouns;
    [Header("暴击率与暴击伤害")]
    public FloatStat critRate;
    public FloatStat critDamage;
    [Header("冲击力")]
    public IntStat impact;
    public FloatStat impactBouns;
    [Header("穿透")]
    public IntStat penetration;
    public FloatStat penetrationRate;
    [Header("异常相关")]
    public IntStat anomalyMastery;
    public IntStat anomalyProficiency;
    [Header("物理伤害相关")]
    public FloatStat physicalDamageBouns;
    [Header("火属性相关")]
    public FloatStat fireDamageBouns;
    [Header("冰属性相关")]
    public FloatStat iceDamageBouns;
    [Header("电属性相关")]
    public FloatStat electricDamageBouns;
    #endregion


    private StringBuilder equipmentStats;

    [Header("制造材料")]
    public List<InventoryItem> craftingMaterials;

    public void ExecuteItemEffect(Transform _target)
    {
        foreach(var item in itemEffects)
        {
            item.ExecuteEffect(_target);
        }
    }

    public void AddModifiers()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        // TODO: 装备提供属性加成


    }

    public void RemoveModifiers()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();


    }

    public override string GetDescription()
    {
        return base.GetDescription();
    }

    public string GetEquipmentStatsDescription()
    {
        equipmentStats.Clear();

        AddIntItemDescription(health.GetValue(), "生命值");
        AddFloatItemDescription(healthBouns.GetValue(), "生命值");

        AddIntItemDescription(attack.GetValue(), "攻击力");
        AddFloatItemDescription(attackBouns.GetValue(), "攻击力");

        AddIntItemDescription(defence.GetValue(), "防御力");
        AddFloatItemDescription(defenceBouns.GetValue(), "防御力");

        AddFloatItemDescription(critRate.GetValue(), "暴击率");
        AddFloatItemDescription(critDamage.GetValue(), "暴击伤害");

        AddIntItemDescription(impact.GetValue(), "冲击力");
        AddFloatItemDescription(impactBouns.GetValue(), "冲击力");

        AddIntItemDescription(penetration.GetValue(), "穿透值");
        AddFloatItemDescription(penetrationRate.GetValue(), "穿透率");

        AddIntItemDescription(anomalyMastery.GetValue(), "异常掌控");
        AddIntItemDescription(anomalyProficiency.GetValue(), "异常精通");

        AddFloatItemDescription(physicalDamageBouns.GetValue(), "物理伤害加成");
        AddFloatItemDescription(fireDamageBouns.GetValue(), "火属性伤害加成");
        AddFloatItemDescription(iceDamageBouns.GetValue(), "冰属性伤害加成");
        AddFloatItemDescription(electricDamageBouns.GetValue(), "电属性伤害加成");

        return equipmentStats.ToString();
    }

    private void AddIntItemDescription(int _value, string _name)
    {
        if (_value != 0)
        {
            if (itemDescription.Length > 0)
            {
                equipmentStats.AppendLine();
            }
            if (_value > 0)
            {
                equipmentStats.Append("+ " + _value + " " + _name);
            }
        }
    }
    private void AddFloatItemDescription(float _value, string _name)
    {
        if (_value != 0)
        {
            if (itemDescription.Length > 0)
            {
                equipmentStats.AppendLine();
            }
            if (_value > 0)
            {
                equipmentStats.Append("+ " + _value * 100 + "% " + _name);
            }
        }
    }

    public List<InventoryItem> GetCraftingMaterials() => craftingMaterials;
    
}
