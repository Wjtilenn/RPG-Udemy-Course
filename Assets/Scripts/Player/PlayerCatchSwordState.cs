using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCatchSwordState : PlayerState
{
    private Transform sword;

    public PlayerCatchSwordState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        sword = SkillManager.instance.sword.sword.transform;
        if ((sword.position.x - player.transform.position.x) * player.facingDir < 0)
        {
            player.Flip();
        }

        rb.velocity = new Vector2(player.swordReturnImpact * -player.facingDir, rb.velocity.y);

        player.fx.PlayDustFX();
        player.fx.ScreenShake();
    }

    public override void Exit()
    {
        base.Exit();

        player.StartCoroutine("BusyFor", .1f);
    }

    public override void Update()
    {
        base.Update();

        if (triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }

    }
}
