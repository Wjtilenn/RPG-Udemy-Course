using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeDazeState : EnemyState
{
    private Slime enemy;
    public SlimeDazeState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Slime _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = ((EnemyStats)enemy.stats).dazeDuration;
    }

    public override void Exit()
    {
        base.Exit();

        ((EnemyStats)enemy.stats).ExitDazeState();
    }

    public override void Update()
    {
        base.Update();
        enemy.SetZeroVelocity();
        if(stateTimer < 0)
        {
            stateMachine.ChangeState(enemy.battleState);
        }
    }
}
