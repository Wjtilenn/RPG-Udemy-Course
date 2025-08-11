using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlockState : PlayerState
{

    public PlayerBlockState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        if (player.skill.counterAttack.CanUseSkill())
        {
            stateTimer = player.skill.counterAttack.counterAttackTime;
        }
        else
        {
            stateTimer = 0;
        }

        player.anim.SetBool("SuccessfulCounterAttack", false);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        player.SetZeroVelocity();

        Collider2D[] collders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        foreach (var hit in collders)
        {
            if (hit.GetComponent<Arrow_Controller>() != null)
            {
                hit.GetComponent<Arrow_Controller>().FlipArrow();
                player.anim.SetBool("SuccessfulCounterAttack", true);
            }
        }

        if (stateTimer > 0)
        {

            foreach (var hit in collders)
            {
                if (hit.GetComponent<Enemy>() != null)
                {
                    if (hit.GetComponent<Enemy>().CanBeStunned())
                    {
                        player.anim.SetBool("SuccessfulCounterAttack", true);
                    }
                }
            }
        }

        if(Input.GetKeyUp(KeyCode.Mouse1) || triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
