using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalAttack_Skill : Skill
{
    [Header("普通攻击信息")]
    [SerializeField] private List<float> attackMultiplier;
    [SerializeField] private List<float> impactMultiplier;
    [SerializeField] private List<int> anomalyAccumulationValue;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    public override bool CanUseSkill(SkillType _skillType = SkillType.None)
    {
        return base.CanUseSkill(_skillType);
    }

    public override void UseSkill(SkillType _skillType = SkillType.None)
    {
        base.UseSkill(_skillType);

        Collider2D[] collders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        foreach (var hit in collders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                Enemy enemy = hit.GetComponent<Enemy>();
                EnemyStats target = hit.GetComponent<EnemyStats>();
                // TODO: 根据武器属性造成对应属性的伤害
                switch (_skillType)
                {
                    case SkillType.NormalAttack1:
                        playerStats.DoDamage(target, attackMultiplier[0], DamageType.Physical);
                        playerStats.DoDaze(target, impactMultiplier[0]);
                        playerStats.DoAnomaly(target, anomalyAccumulationValue[0], DamageType.Physical);
                        break;
                    case SkillType.NormalAttack2:
                        playerStats.DoDamage(target, attackMultiplier[1], DamageType.Physical);
                        playerStats.DoDaze(target, impactMultiplier[1]);
                        playerStats.DoAnomaly(target, anomalyAccumulationValue[1], DamageType.Fire);
                        break;
                    case SkillType.NormalAttack3:
                        playerStats.DoDamage(target, attackMultiplier[2], DamageType.Physical);
                        playerStats.DoDaze(target, impactMultiplier[2]);
                        playerStats.DoAnomaly(target, anomalyAccumulationValue[2], DamageType.Ice);
                        break;
                    default:
                        break;
                }
                enemy.SetupKnockbackDir(player.transform);
                // TODO: 攻击时武器效果

            }
        }

        
    }

}
