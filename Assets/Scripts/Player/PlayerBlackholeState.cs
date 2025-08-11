using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlackholeState : PlayerState
{
    private float flyTime = 0.4f;
    private bool skillUsed;

    private float defaultGravity;

    public PlayerBlackholeState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.skill.blackhole.skillFinish = false;
        defaultGravity = player.rb.gravityScale;
        skillUsed = false;
        stateTimer = flyTime;
        rb.gravityScale = 0;

    }

    public override void Exit()
    {
        base.Exit();
        player.rb.gravityScale = defaultGravity;
        player.fx.MakeTransprent(false);
    }

    public override void Update()
    {
        base.Update();

        if(stateTimer > 0)
        {
            rb.velocity = new Vector2(0, 5);
        }

        if(stateTimer < 0)
        {
            rb.velocity = Vector2.zero;
            if (!skillUsed)
            {
                player.fx.MakeTransprent(true);
                player.skill.blackhole.UseSkill();
                skillUsed = true;
            }
            if(Input.GetKeyDown(SkillManager.instance.ultimateAbility.key))
            {
                player.skill.blackhole.UseSkill();
            }
        }

        if (SkillManager.instance.blackhole.skillFinish)
        {
            stateMachine.ChangeState(player.airState);
        }
        
    }
}
