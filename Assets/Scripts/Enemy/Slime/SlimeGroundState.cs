using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeGroundState : EnemyState
{
    protected Slime enemy;
    protected Transform player;
    public SlimeGroundState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Slime _ememy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _ememy;
    }

    public override void Enter()
    {
        base.Enter();
        player = PlayerManager.instance.player.transform;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if(enemy.IsplayerDetected() || Vector2.Distance(player.position, enemy.transform.position) < 2)
        {
            stateMachine.ChangeState(enemy.battleState);
        }
    }
}
