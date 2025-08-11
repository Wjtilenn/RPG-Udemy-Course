using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();
    private void AnimationTrigger()
    {   
        player.AnimationTrigger();
    }

    private void AttackTrigger()
    {
        player.skill.normalAttack.UseSkill(SkillType.NormalAttack1 + player.primaryAttackState.comboCounter);
    }

    private void ThrowSword()
    {
        SkillManager.instance.sword.CreateSword();
    }

    private void PhantomPursuit()
    {
        SkillManager.instance.counterAttack.UseSkill();
    }
}
