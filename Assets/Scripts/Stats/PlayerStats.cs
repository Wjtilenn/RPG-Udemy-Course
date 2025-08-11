using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerStats : CharacterStats
{
    private Player player;

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
    public float physicalAnomalyDuration;
    [Header("火属性相关")]
    public FloatStat fireDamageBouns;
    public float fireAnomalyDuration;
    public float burnInterval;
    [Header("冰属性相关")]
    public FloatStat iceDamageBouns;
    public float iceAnomalyDuration;
    [Header("电属性相关")]
    public FloatStat electricDamageBouns;
    public float electricAnomalyDuration;
    public float electricShockInterval;


    protected override void Start()
    {
        base.Start();

        player = GetComponent<Player>();
    }
    #region 伤害计算
    public override void DoDamage(CharacterStats _targetStats, float _multiplier, DamageType _damageType)
    {
        base.DoDamage(_targetStats, _multiplier, _damageType);
        /*
            伤害 = 基础伤害区 * 增伤区 * 暴击区 * 防御区 * 抗性区 * 失衡区
            基础伤害区 = 对应属性 * 伤害倍率
            增伤区 = (1 + 属性伤害增伤 + buff增伤 + 全类型增伤)
            暴击区 = (暴击 ? 1 + 爆伤 : 1)
            防御区 = 攻击方等级基数 / (受击方有效防御 + 攻击方等级基数)
                                       受击方有效防御 = 受击方防御 * (1 - 攻击方穿透率) - 攻击方穿透值 >= 0
            抗性区 = (1 - 受击方对应属性抗性)
            失衡区 = 敌方失衡 ? 1 + 失衡倍率 : 1
        */
        EnemyStats targetStats = _targetStats as EnemyStats;
        float damage = _multiplier * GetAttack();
        switch (_damageType)
        {
            case DamageType.Physical:
                damage = damage * (1.0f + physicalDamageBouns.GetValue());
                break;
            case DamageType.Fire:
                damage = damage * (1.0f + fireDamageBouns.GetValue());
                break;
            case DamageType.Ice:
                damage = damage * (1.0f + iceDamageBouns.GetValue());
                break;
            case DamageType.Electric:
                damage = damage * (1.0f + electricDamageBouns.GetValue());
                break;
            default:
                break;
        }

        if (Random.value < critRate.GetValue())
        {
            damage = damage * (1.0f + critDamage.GetValue());
        }

        float validDefence = Mathf.Max(0f, targetStats.GetDefence() * (1.0f - penetrationRate.GetValue()) - penetration.GetValue());
        damage = damage * GetLevelBase() / (GetLevelBase() + validDefence);

        switch (_damageType)
        {
            case DamageType.Physical:
                damage = damage * (1.0f - targetStats.physicalDamageResistance.GetValue());
                break;
            case DamageType.Fire:
                damage = damage * (1.0f - targetStats.fireDamageResistance.GetValue());
                break;
            case DamageType.Ice:
                damage = damage * (1.0f - targetStats.iceDamageResistance.GetValue());
                break;
            case DamageType.Electric:
                damage = damage * (1.0f - targetStats.electricDamageResistance.GetValue());
                break;
            default:
                break;
        }

        if (targetStats.isDaze) damage = damage * (1.0f + targetStats.dazeMultiplier);

        _targetStats.TakeDamage(Mathf.FloorToInt(damage));
        fx.CreateHitFX(_targetStats.transform);
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
    }

    public override void IncreaseHealthBy(int _amount)
    {
        base.IncreaseHealthBy(_amount);
    }

    protected override void DecreaseHealthBy(int _damage)
    {
        base.DecreaseHealthBy(_damage);
    }
    #endregion

    #region 失衡计算

    public override void DoDaze(CharacterStats _target, float _impactMultiplier)
    {
        /*
            失衡值 = 冲击区 * 失衡倍率 
            冲击区 = 冲击力 * (100 + 冲击加成) / 100
        */
        float daze = GetImpact() * _impactMultiplier;
        _target.TakeDaze(daze);

    }
    public override void TakeDaze(float _daze)
    {

    }
    #endregion

    #region 异常计算

    public void DoAnomaly(EnemyStats _target, int _anomalyAccumulationValue, DamageType _type)
    {
        int value = Mathf.FloorToInt(_anomalyAccumulationValue * anomalyMastery.GetValue() / 100.0f);
        _target.TakeAnomaly(value, _type);
    }

    #endregion

    #region 属性获取
    public override int GetMaxHealth() => Mathf.FloorToInt(health.GetValue() * (1.0f + healthBouns.GetValue()));
    public override float GetAttack() => attack.GetValue() * (1.0f + attackBouns.GetValue());
    public override float GetDefence() => defence.GetValue() * (1.0f + defenceBouns.GetValue());
    public override float GetLevelBase() => 0.1551f * level * level + 3.141f * level + 47.2039f;
    public float GetImpact() => impact.GetValue() * (1.0f + impactBouns.GetValue());
    public float GetPhysicalAnomalyMultiplier() => (200.0f + 200.0f * (level - 1) / 59.0f) / 100.0f;
    public float GetFireAnomalyMultiplier() => (30.0f + 20.0f * (level - 1) / 59.0f) / 100.0f;
    public float GetIceAnomalyMultiplier() => (150.0f + 200.0f * (level - 1) / 59.0f) / 100.0f;
    public float GetElectricAnomalyMultipler() => (35.0f + 20.0f * (level - 1) / 59.0f) / 100.0f;
    #endregion

    public override void Die()
    {
        base.Die();

        player.Die();

        GetComponent<PlayerItemDrop>().GenerateDrop();
    }
}
