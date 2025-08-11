using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThrowSwordState : PlayerState
{
    public PlayerThrowSwordState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

    }

    public override void Exit()
    {
        base.Exit();
        SkillManager.instance.sword.ExitAiming();
        player.StartCoroutine("BusyFor", 0.2f);
    }

    public override void Update()
    {
        base.Update();

        player.SetZeroVelocity();

        foreach (var skill in SkillManager.instance.gameInterfaceSkillBar)
        {
            if (Input.GetKeyUp(skill.key))
            {
                if (skill.skillType == SkillType.Throw || skill.skillType == SkillType.Bounce
                    || skill.skillType == SkillType.Pierce || skill.skillType == SkillType.Spin)
                {
                    stateMachine.ChangeState(player.idleState);
                }
            }
        }

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if((mousePosition.x - player.transform.position.x) * player.facingDir < 0)
        {
            player.Flip();
        }
    }
}
