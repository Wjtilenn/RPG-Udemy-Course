using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dash_Skill : Skill
{
    [Header("冲刺信息")]
    public float dashSpeed;
    public float defaultDashSpeed;
    public float dashDuration;
    public float dashDir;

    public bool dashUnlocked { get; private set; }
    [SerializeField] private UI_SkillSlot dashUnlockButton;

    public bool phantomDashUnlocked { get; private set; }
    [Header("幻影冲刺")]
    [SerializeField] private UI_SkillSlot phantomDashUnlockButton;

    public bool phantomTrailUnlocked { get; private set; }
    [Header("幻影轨迹")]
    [SerializeField] private UI_SkillSlot phantomTrailUnlockButton;

    protected override void Start()
    {
        base.Start();
        dashUnlockButton.unlockSkill.AddListener(UnlockDash);
        phantomDashUnlockButton.unlockSkill.AddListener(UnlockPhantomDash);
        phantomTrailUnlockButton.unlockSkill.AddListener(UnlockPhantomTrail);
    }
    public override bool CanUseSkill(SkillType _type = SkillType.None)
    {
        return dashUnlocked && base.CanUseSkill();
    }

    public override void UseSkill(SkillType _skillType = SkillType.None)
    {
        base.UseSkill(_skillType);
        dashDir = Input.GetAxisRaw("Horizontal");
        if (dashDir == 0)
        {
            dashDir = player.facingDir;
        }

        if (phantomTrailUnlocked)
        {
            PhantomTrail();
            return;
        }
        if (phantomDashUnlocked)
        {
            PhantomDash();
            return;
        }
    }

    public void PhantomDash()
    {
        SkillManager.instance.clone.CreateClone(player.transform, Vector3.zero);
    }

    public void PhantomTrail()
    {
        for(int i = 0;i < 3; i++)
        {
            StartCoroutine(CreateCloneWithDelay(dashDuration / 3 * i));
        }
    }

    private IEnumerator CreateCloneWithDelay(float _seconds) 
    {
        yield return new WaitForSeconds(_seconds);
        SkillManager.instance.clone.CreateClone(player.transform , Vector3.zero);
    }

    #region 解锁技能
    private void UnlockDash()
    {
        if(dashUnlockButton.unlocked)
        {
            dashUnlocked = true;
        }
    }

    private void UnlockPhantomDash()
    {
        if (phantomDashUnlockButton.unlocked)
        {
            phantomDashUnlocked = true;
        }
    }

    private void UnlockPhantomTrail()
    {
        if (phantomTrailUnlockButton.unlocked)
        {
            phantomTrailUnlocked = true;
        }
    }
    #endregion

}
