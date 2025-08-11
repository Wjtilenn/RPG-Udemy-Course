using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;


public class EnemyStats : CharacterStats
{
    private Enemy enemy;
    private PlayerStats playerStats;
    
    private ItemDrop myDropSystem;
    [Header("失衡信息")]
    public bool isDaze;
    public float dazeDuration = 3;
    public float dazeMultiplier = 0.5f;
    public int dazeBar;
    public float currentDazeRate;

    [Header("物理属性异常")]
    public FloatStat physicalDamageResistance;
    public int physicalAnomalyBar = 1200;
    public int currentPhysicalAnomaly;
    public bool isPhysicalAnomaly;
    private float physicalAnomalyTimer;
    [Header("火属性异常")]
    public FloatStat fireDamageResistance;
    public int fireAnomalyBar = 1000;
    public int currentFireAnomaly;
    public bool isFireAnomaly;
    private float fireAnomalyTimer;
    private float burnTimer;
    [Header("冰属性异常")]
    public FloatStat iceDamageResistance;
    public int iceAnomalyBar = 1000;
    public int currentIceAnomaly;
    public bool isIceAnomaly;
    private float iceAnomalyTimer;
    [Header("电属性异常")] 
    public FloatStat electricDamageResistance;
    public int electricAnomalyBar = 1000;
    public int currentElectricAnomaly;
    public bool isElectricAnomaly;
    private float electricAnomalyTimer;
    private float electricShockTimer;

    private UnityEvent onTakeDamage;

    protected override void Start()
    {
        base.Start();

        enemy = GetComponent<Enemy>();
        playerStats = PlayerManager.instance.player.stats as PlayerStats;
        myDropSystem = GetComponent<ItemDrop>();
        
        isDaze = false;

        onTakeDamage = new UnityEvent();
        onTakeDamage.AddListener(SufferElectricShockDamage);
    }

    protected override void Update()
    {
        base.Update();
        if (isPhysicalAnomaly)
        {
            physicalAnomalyTimer -= Time.deltaTime;
            if(physicalAnomalyTimer < 0) ExitPhysicalAnomaly();
        }
        if (isFireAnomaly)
        {
            fireAnomalyTimer -= Time.deltaTime;
            burnTimer -= Time.deltaTime;
            if(burnTimer < 0) SufferBurnDamage();
            
            if (fireAnomalyTimer < 0) ExitFireAnomaly();
        }
        if (isIceAnomaly)
        {
            iceAnomalyTimer -= Time.deltaTime;
            if(iceAnomalyTimer < 0) ExitIceAnomaly();
        }
        if (isElectricAnomaly)
        {
            electricAnomalyTimer -= Time .deltaTime;
            electricShockTimer -= Time .deltaTime;

            if(electricAnomalyTimer < 0) ExitElectricAnomaly();
        }
    }

    #region 伤害计算
    public override void DoDamage(CharacterStats _targetStats, float _multiplier, DamageType _damageType)
    {
        base.DoDamage(_targetStats, _multiplier, _damageType);
        /*
            伤害 = 基础伤害区 * 增伤区 * 暴击区 * 防御区 * 抗性区
            基础伤害区 = 对应属性 * 伤害倍率
            防御区 = 攻击方等级基数 / (受击方有效防御 + 攻击方等级基数)
        */
        float damage = _multiplier * GetAttack();

        float validDefence = Mathf.Max(0f, _targetStats.GetDefence());   
        damage = damage * GetLevelBase() / (GetLevelBase() + validDefence);

        _targetStats.TakeDamage(Mathf.FloorToInt(damage));
        fx.CreateHitFX(_targetStats.transform);

    }
    
    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);

        onTakeDamage?.Invoke();
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
        
    }

    public override void TakeDaze(float _daze)
    {
        if (isDaze || _daze <= 0) return;

        float add = _daze / dazeBar;
        currentDazeRate += add;

        if(currentDazeRate >= 1)
        {
            currentDazeRate = 0;

            isDaze = true;
            enemy.EnterDazeState();
        }
    }

    public override void ExitDazeState()
    {
        isDaze = false;
    }
    #endregion

    #region 异常积蓄计算

    public void TakeAnomaly(int _value, DamageType _type)
    {
        switch (_type)
        {
            case DamageType.Physical:
                PhysicalAnomalyAccumulation(_value);
                break;
            case DamageType.Fire:
                FireAnomalyAccmulation(_value);
                break;
            case DamageType.Ice:
                IceAnomalyAccmulation(_value);
                break;
            case DamageType.Electric:
                ElectricAnomalyAccmulation(_value);
                break;
            default:
                break;
        }
    }

    private void PhysicalAnomalyAccumulation(int _value)
    {
        currentPhysicalAnomaly = Mathf.Clamp(currentPhysicalAnomaly + _value, 0, physicalAnomalyBar);
        if (currentPhysicalAnomaly == physicalAnomalyBar)
        {
            EnterPhysicalAnomaly();
            currentPhysicalAnomaly = 0;
        }
    }
    private void FireAnomalyAccmulation(int _value)
    {
        currentFireAnomaly = Mathf.Clamp(currentFireAnomaly + _value, 0, fireAnomalyBar);
        if (currentFireAnomaly == fireAnomalyBar)
        {
            EnterFireAnomaly();
            currentFireAnomaly = 0;
        }
    }
    private void IceAnomalyAccmulation(int _value)
    {
        currentIceAnomaly = Mathf.Clamp(currentIceAnomaly + _value, 0, iceAnomalyBar);
        if (currentIceAnomaly == iceAnomalyBar)
        {
            EnterIceAnomaly();
            currentIceAnomaly = 0;
        }
    }
    private void ElectricAnomalyAccmulation(int _value)
    {
        currentElectricAnomaly = Mathf.Clamp(currentElectricAnomaly + _value, 0, electricAnomalyBar);
        if (currentElectricAnomaly == electricAnomalyBar)
        {
            EnterElectricAnomaly();
            currentElectricAnomaly = 0;
        }
    }
    #endregion

    #region 异常伤害计算
    /*
        异常伤害 = 基础区 * 增伤区 * 失衡易伤区 * 异常精通区 * 抗性区 * 防御区
        基础区 = 角色攻击力 * 异常伤害倍率
        增伤区 = (1 + 属性伤害增伤 + buff增伤 + 全类型增伤)
        失衡易伤区 = 失衡 ? 1 + 失衡易伤 : 1
        防御区 = 攻击方等级基数 / (受击方有效防御 + 攻击方等级基数)
                                   受击方有效防御 = 受击方防御 * (1 - 攻击方穿透率) - 攻击方穿透值 >= 0
        抗性区 = (1 - 受击方对应属性抗性)
    */

    #region 物理异常 强击伤害相关
    private void EnterPhysicalAnomaly()
    {
        DisorderDamage(DamageType.Physical);

        SufferAssaultDamage();

        isPhysicalAnomaly = true;
        physicalAnomalyTimer = playerStats.physicalAnomalyDuration;
    }

    private void SufferAssaultDamage()
    {
        float damage = 0;
        damage = playerStats.GetPhysicalAnomalyMultiplier() * playerStats.GetAttack();

        damage = damage * playerStats.anomalyProficiency.GetValue() / 100.0f;

        if (isDaze) damage = damage * (1.0f + dazeMultiplier);

        float validDefence = Mathf.Max(0f, GetDefence() * (1.0f - playerStats.penetrationRate.GetValue()) - playerStats.penetration.GetValue());
        damage = damage * playerStats.GetLevelBase() / (playerStats.GetLevelBase() + validDefence);

        damage = damage * (1.0f - physicalDamageResistance.GetValue());

        DecreaseHealthBy(Mathf.FloorToInt(damage));
    }

    private void ExitPhysicalAnomaly()
    {
        isPhysicalAnomaly = false;
    }
    #endregion

    #region 火异常 燃烧伤害相关
    private void EnterFireAnomaly()
    {
        DisorderDamage(DamageType.Fire);

        isFireAnomaly = true;
        fireAnomalyTimer = playerStats.fireAnomalyDuration;
    }

    private void SufferBurnDamage()
    {
        float damage = 0;
        damage = playerStats.GetFireAnomalyMultiplier() * playerStats.GetAttack();

        damage = damage * playerStats.anomalyProficiency.GetValue() / 100.0f;

        if (isDaze) damage = damage * (1.0f + dazeMultiplier);

        float validDefence = Mathf.Max(0f, GetDefence() * (1.0f - playerStats.penetrationRate.GetValue()) - playerStats.penetration.GetValue());
        damage = damage * playerStats.GetLevelBase() / (playerStats.GetLevelBase() + validDefence);

        damage = damage * (1.0f - fireDamageResistance.GetValue());

        DecreaseHealthBy(Mathf.FloorToInt(damage));

        burnTimer = playerStats.burnInterval;
    }

    private void ExitFireAnomaly()
    {
        isFireAnomaly = false;
    }
    #endregion

    #region 冰异常 碎冰伤害相关
    private void EnterIceAnomaly()
    {
        DisorderDamage(DamageType.Ice);

        SufferCrushedIceDamage();

        isIceAnomaly = true;
        iceAnomalyTimer = playerStats.iceAnomalyDuration;
    }

    private void SufferCrushedIceDamage()
    {
        float damage = 0;
        damage = playerStats.GetIceAnomalyMultiplier() * playerStats.GetAttack();

        damage = damage * playerStats.anomalyProficiency.GetValue() / 100.0f;

        if (isDaze) damage = damage * (1.0f + dazeMultiplier);

        float validDefence = Mathf.Max(0f, GetDefence() * (1.0f - playerStats.penetrationRate.GetValue()) - playerStats.penetration.GetValue());
        damage = damage * playerStats.GetLevelBase() / (playerStats.GetLevelBase() + validDefence);

        damage = damage * (1.0f - iceDamageResistance.GetValue());

        DecreaseHealthBy(Mathf.FloorToInt(damage));

    }

    private void ExitIceAnomaly()
    {
        isIceAnomaly = false;
    }
    #endregion

    #region 电异常 感电伤害相关
    private void EnterElectricAnomaly()
    {
        DisorderDamage(DamageType.Electric);

        isElectricAnomaly = true;
        electricAnomalyTimer = playerStats.electricAnomalyDuration;
    }

    private void SufferElectricShockDamage()
    {
        if (!isElectricAnomaly || electricShockTimer > 0) return;

        float damage = 0;
        damage = playerStats.GetElectricAnomalyMultipler() * playerStats.GetAttack();

        damage = damage * playerStats.anomalyProficiency.GetValue() / 100.0f;

        if (isDaze) damage = damage * (1.0f + dazeMultiplier);

        float validDefence = Mathf.Max(0f, GetDefence() * (1.0f - playerStats.penetrationRate.GetValue()) - playerStats.penetration.GetValue());
        damage = damage * playerStats.GetLevelBase() / (playerStats.GetLevelBase() + validDefence);

        damage = damage * (1.0f - electricDamageResistance.GetValue());

        DecreaseHealthBy(Mathf.FloorToInt(damage));

        electricShockTimer = playerStats.electricShockInterval;

    }

    private void ExitElectricAnomaly()
    {
        isElectricAnomaly = false;
    }
    #endregion

    #region 紊乱伤害结算
    private void DisorderDamage(DamageType _type)
    {
        // 物理异常紊乱 再次受到一次强击伤害
        if (isPhysicalAnomaly)
        {
            if (_type == DamageType.Physical) return;
            float damage = 0;
            damage = playerStats.GetPhysicalAnomalyMultiplier() * playerStats.GetAttack();

            damage = damage * playerStats.anomalyProficiency.GetValue() / 100.0f;

            if (isDaze) damage = damage * (1.0f + dazeMultiplier);

            float validDefence = Mathf.Max(0f, GetDefence() * (1.0f - playerStats.penetrationRate.GetValue()) - playerStats.penetration.GetValue());
            damage = damage * playerStats.GetLevelBase() / (playerStats.GetLevelBase() + validDefence);

            damage = damage * (1.0f - physicalDamageResistance.GetValue());

            DecreaseHealthBy(Mathf.FloorToInt(damage));
            Debug.Log("紊乱 " + Mathf.FloorToInt(damage));

            ExitPhysicalAnomaly();
        }
        // 火属性异常紊乱结算 根据剩余时间受到伤害
        if (isFireAnomaly)
        {
            if (_type == DamageType.Fire) return;
            float damage = 0;
            damage = playerStats.GetFireAnomalyMultiplier() * 10 * (1f + Mathf.FloorToInt(fireAnomalyTimer / 0.5f) / 20f) * playerStats.GetAttack();

            damage = damage * playerStats.anomalyProficiency.GetValue() / 100.0f;

            if (isDaze) damage = damage * (1.0f + dazeMultiplier);

            float validDefence = Mathf.Max(0f, GetDefence() * (1.0f - playerStats.penetrationRate.GetValue()) - playerStats.penetration.GetValue());
            damage = damage * playerStats.GetLevelBase() / (playerStats.GetLevelBase() + validDefence);

            damage = damage * (1.0f - fireDamageResistance.GetValue());

            DecreaseHealthBy(Mathf.FloorToInt(damage));
            Debug.Log("紊乱 " + Mathf.FloorToInt(damage));

            ExitFireAnomaly();
        }
        // 冰属性异常紊乱结算 再次受到一次碎冰伤害
        if (isIceAnomaly)
        {
            if (_type == DamageType.Ice) return;
            float damage = 0;
            damage = playerStats.GetIceAnomalyMultiplier() * playerStats.GetAttack();

            damage = damage * playerStats.anomalyProficiency.GetValue() / 100.0f;

            if (isDaze) damage = damage * (1.0f + dazeMultiplier);

            float validDefence = Mathf.Max(0f, GetDefence() * (1.0f - playerStats.penetrationRate.GetValue()) - playerStats.penetration.GetValue());
            damage = damage * playerStats.GetLevelBase() / (playerStats.GetLevelBase() + validDefence);

            damage = damage * (1.0f - iceDamageResistance.GetValue());

            DecreaseHealthBy(Mathf.FloorToInt(damage));
            Debug.Log("紊乱 " + Mathf.FloorToInt(damage));

            ExitIceAnomaly();
        }
        // 电属性异常紊乱结算 根据异常剩余时间结算伤害
        if (isElectricAnomaly)
        {
            if(_type == DamageType.Electric) return;
            float damage = 0;
            damage = playerStats.GetElectricAnomalyMultipler() * 10 * (1 + Mathf.FloorToInt(electricAnomalyTimer / 0.5f) / 20.0f) * playerStats.GetAttack();

            damage = damage * playerStats.anomalyProficiency.GetValue() / 100.0f;

            if (isDaze) damage = damage * (1.0f + dazeMultiplier);

            float validDefence = Mathf.Max(0f, GetDefence() * (1.0f - playerStats.penetrationRate.GetValue()) - playerStats.penetration.GetValue());
            damage = damage * playerStats.GetLevelBase() / (playerStats.GetLevelBase() + validDefence);

            damage = damage * (1.0f - electricDamageResistance.GetValue());

            DecreaseHealthBy(Mathf.FloorToInt(damage));
            Debug.Log("紊乱 " + Mathf.FloorToInt(damage));

            ExitElectricAnomaly();
        }
    }
    #endregion

    #endregion

    public override void Die()
    {
        if (isDead) return;
        base.Die();
        enemy.Die();

        myDropSystem.GenerateDrop();
        Destroy(gameObject, 5.0f);
    }
}
