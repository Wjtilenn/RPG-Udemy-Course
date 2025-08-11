using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBattleState : EnemyState
{
    private Transform player;
    private Skeleton enemy;
    private int moveDir;
    public SkeletonBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Skeleton _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.instance.player.transform;

        if (player.GetComponent<PlayerStats>().isDead)
        {
            stateMachine.ChangeState(enemy.moveState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        moveDir = player.position.x > enemy.transform.position.x ? 1 : -1;

        if (Vector2.Distance(player.position, enemy.transform.position) > 1)
        {
            enemy.SetVelocity(enemy.moveSpeed * moveDir, rb.velocity.y);
            ChangeAnimationState("Move");
        }
        else
        {
            if ((player.transform.position.x - enemy.transform.position.x) * enemy.facingDir < 0)
            {
                enemy.Flip();
            }
            ChangeAnimationState("Idle");
        }

        if (enemy.IsplayerDetected())
        {
            stateTimer = enemy.battleTime;
            if(CanAttack() && enemy.IsplayerDetected().distance < enemy.attackDistance)
            {
                stateMachine.ChangeState(enemy.attackState);
            }
        }
        else
        {
            if(stateTimer < 0)
            {
                stateMachine.ChangeState(enemy.idleState);
            }
        }

    }

    private bool CanAttack()
    {
        if(Time.time - enemy.lastTimeAttacked >= enemy.attackCooldown) return true;
        return false; 
    }

    private void ChangeAnimationState(string _animationNmae)
    {
        enemy.anim.SetBool(animBoolName, false);
        animBoolName = _animationNmae;
        enemy.anim.SetBool(animBoolName, true);
    }
}
