using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherBattleState : EnemyState
{
    private Archer enemy;
    private Transform player;

    private int moveDir;

    public ArcherBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Archer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        player = PlayerManager.instance.player.transform;

        stateTimer = enemy.battleTime;

        if (player.GetComponent<PlayerStats>().isDead)
        {
            stateMachine.ChangeState(enemy.idleState);
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
        float distanceToPlayer = Vector2.Distance(player.position, enemy.transform.position);
        if (distanceToPlayer > enemy.attackDistance)
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
            if (distanceToPlayer < 4 && CanJump())
            {
                stateMachine.ChangeState(enemy.jumpState);
                return;
            }   
            else
            {
                ChangeAnimationState("Idle");
            }
        }

        if (enemy.IsplayerDetected())
        {
            stateTimer = enemy.battleTime;

            if (CanAttack() && enemy.IsplayerDetected().distance < enemy.attackDistance)
            {
                stateMachine.ChangeState(enemy.attackState);
            }
        }
        else
        {
            if (stateTimer < 0)
            {
                stateMachine.ChangeState(enemy.idleState);
            }
        }


    }

    private bool CanAttack()
    {
        if (Time.time - enemy.lastTimeAttacked >= enemy.attackCooldown) return true;
        return false;
    }

    private bool CanJump()
    {
        if(Time.time - enemy.lastJumpTime >= enemy.jumpCooldown) return true;
        return false;
    }

    private void ChangeAnimationState(string _animationNmae)
    {
        enemy.anim.SetBool(animBoolName, false);
        animBoolName = _animationNmae;
        enemy.anim.SetBool(animBoolName, true);
    }
}
