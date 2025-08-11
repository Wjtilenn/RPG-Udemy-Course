using System.Collections;
using UnityEngine;

public enum DamageType
{
    Physical,
    Fire,
    Ice,
    Electric
}

public class CharacterStats : MonoBehaviour
{
    protected EntityFX fx;

    public int level;

    #region 角色属性
    [Header("生命值")]
    public IntStat health;
    public FloatStat healthBouns;
    public int currentHealth;
    [Header("攻击力")]
    public IntStat attack;
    public FloatStat attackBouns;
    [Header("防御力")]
    public IntStat defence;
    public FloatStat defenceBouns;
    #endregion


    public bool isDead {  get; private set; }


    public System.Action onHealthChanged;
    
    protected virtual void Start()
    {
        fx = GetComponent<EntityFX>();
        currentHealth = health.GetValue();
    }

    protected virtual void Update()
    {

    }

    public virtual void IncreaseStatBy(int _modifier, float _duration, IntStat _statToModify)
    {
        StartCoroutine(StatModCoroutine(_modifier, _duration, _statToModify));
    }
    
    private IEnumerator StatModCoroutine(int _modifier, float _duration, IntStat _statToModify)
    {
        _statToModify.AddModifier(_modifier);
        
        yield return new WaitForSeconds(_duration);

        _statToModify.RemoveModifier(_modifier);
    }

    #region 伤害计算
    public virtual void DoDamage(CharacterStats _targetStats, float _multiplier, DamageType _damageType)
    {
        
    }

    public virtual void TakeDamage(int _damage)
    {

        GetComponent<Entity>().DamageImpact();

        DecreaseHealthBy(_damage);
        fx.StartCoroutine("FlashFX");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public virtual void IncreaseHealthBy(int _amount)
    {
        currentHealth += _amount;

        if(currentHealth > health.GetValue())
        {
            currentHealth = health.GetValue();
        }

        onHealthChanged?.Invoke();
    }

    protected virtual void DecreaseHealthBy(int _damage)
    {
        currentHealth -= _damage;

        onHealthChanged?.Invoke();
    }

    #endregion

    #region 失衡计算

    public virtual void DoDaze(CharacterStats _target, float _impactMultiplier)
    {

    }

    public virtual void TakeDaze(float _daze)
    {

    }

    public virtual void ExitDazeState()
    {

    }

    #endregion

    #region 属性获取
    public virtual int GetMaxHealth() => Mathf.FloorToInt(health.GetValue() * (1.0f + healthBouns.GetValue()));
    public virtual float GetAttack() => attack.GetValue() * (1.0f + attackBouns.GetValue());
    public virtual float GetDefence() => defence.GetValue() * (1.0f + defenceBouns.GetValue());
    public virtual float GetLevelBase() => 0.1551f * level * level + 3.141f * level + 47.2039f;
    #endregion

    public virtual void Die()
    {
        if (isDead) return;
        isDead = true;
    }

}
