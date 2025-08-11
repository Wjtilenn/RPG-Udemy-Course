using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        foreach(var skill in SkillManager.instance.gameInterfaceSkillBar)
        {
            if (Input.GetKeyDown(skill.key))
            {
                player.ReleaseSkill(skill);
            }
        }

        if (Input.GetKeyDown(SkillManager.instance.ultimateAbility.key))
        {
            if (player.ReleaseSkill(SkillManager.instance.ultimateAbility))
            {
                stateMachine.ChangeState(player.blackholeState);
            }
        }

        if (Input.GetKey(KeyCode.Mouse0))
        {
            stateMachine.ChangeState(player.primaryAttackState);
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            stateMachine.ChangeState(player.blockState);
        }

        if (!player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.airState);
        }

        if (Input.GetKeyDown(KeyCode.Space) && player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.jumpState);
        }
    }

    
}
