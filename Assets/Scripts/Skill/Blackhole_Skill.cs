using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackhole_Skill : Skill
{
    [SerializeField] private GameObject blackholePrefab;
    [SerializeField] private float maxSize;
    [SerializeField] private float growSpeed;
    [SerializeField] private float shrinkSpeed;
    [SerializeField] private float blackholeDuration;
    [SerializeField] int cloneAttackAmount;

    public bool skillFinish;
    Blackhole_Skill_Controller currentBlackhole;

    public override bool CanUseSkill(SkillType _type = SkillType.None)
    {
        return base.CanUseSkill();
    }

    public override void UseSkill(SkillType _skillType = SkillType.None)
    {
        if(currentBlackhole != null)
        {
            currentBlackhole.ReleaseCloneAttack();
        }
        else
        {
            GameObject newBlackhole = Instantiate(blackholePrefab, player.transform.position, Quaternion.identity);
            currentBlackhole = newBlackhole.GetComponent<Blackhole_Skill_Controller>();
            currentBlackhole.SetupBlackhole(maxSize, growSpeed, shrinkSpeed, blackholeDuration, cloneAttackAmount);
            skillFinish = false;
        }
    }

    public void SkillFinish()
    {
        cooldownTimer = cooldown;
        skillFinish = true;
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

}
