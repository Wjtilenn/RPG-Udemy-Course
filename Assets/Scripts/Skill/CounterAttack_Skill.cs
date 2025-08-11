using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterAttack_Skill : Skill
{
    public bool counterAttackUnlocked { get; private set; }
    [Header("格挡反击")]
    public float counterAttackTime;
    [SerializeField] private UI_SkillSlot counterAttackUnlockButton;

    public bool phantomPursuitUnlocked {  get; private set; }
    [Header("幻影追击")]
    [SerializeField] private UI_SkillSlot phantomPursuitUnlockButton;


    protected override void Start()
    {
        base.Start();
        counterAttackUnlockButton.unlockSkill.AddListener(UnlockCounterAttack);
        phantomPursuitUnlockButton.unlockSkill.AddListener(UnlockPhantomPursuit);
    }

    protected override void Update()
    {
        base.Update();
    }

    public override bool CanUseSkill(SkillType _type = SkillType.None)
    {
        return counterAttackUnlocked && base.CanUseSkill();
    }

    public override void UseSkill(SkillType _skillType = SkillType.None)
    {
        base.UseSkill(_skillType);
        // TODO: 格挡反击对敌人造成大量失衡
        if (phantomPursuitUnlocked)
        {
            PhantomPursuit(PlayerManager.instance.player.transform, new Vector3(3 * PlayerManager.instance.player.facingDir, 0, 0));
        }
    }


    private void PhantomPursuit(Transform _position, Vector3 _offset)
    {
        SkillManager.instance.clone.CreateClone(_position, _offset);
    }

    private void UnlockCounterAttack()
    {
        if (counterAttackUnlockButton.unlocked)
        {
            counterAttackUnlocked = true;
        }
    } 

    private void UnlockPhantomPursuit()
    {
        if(phantomPursuitUnlockButton.unlocked)
        {
            phantomPursuitUnlocked = true;
        }
    }
}
