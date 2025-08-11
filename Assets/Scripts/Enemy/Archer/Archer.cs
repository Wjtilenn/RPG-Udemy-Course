using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Enemy
{
    #region 状态定义
    public ArcherIdleState idleState { get; private set; }
    public ArcherAttackState attackState { get; private set; }
    public ArcherBattleState battleState { get; private set; }
    public ArcherMoveState moveState { get; private set; }
    public ArcherJumpState jumpState { get; private set; }
    public ArcherDazeState dazeState { get; private set; }
    public ArcherDeadState deadState { get; private set; }
    #endregion

    [Header("弓箭手信息")]
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform arrowLaunchPoint;
    public float arrowSpeed;
    public Vector2 jumpVelocity;
    public float jumpCooldown;
    [HideInInspector] public float lastJumpTime;

    protected override void Awake()
    {
        base.Awake();

        idleState = new ArcherIdleState(this, stateMachine, "Idle", this);
        attackState = new ArcherAttackState(this, stateMachine, "Attack", this);
        moveState = new ArcherMoveState(this, stateMachine, "Move", this);
        battleState = new ArcherBattleState(this, stateMachine, "Move", this);
        jumpState = new ArcherJumpState(this, stateMachine, "Jump", this);
        dazeState = new ArcherDazeState(this, stateMachine, "Daze", this);
        deadState = new ArcherDeadState(this, stateMachine, "Die", this);
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.initiallize(idleState);
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void Die()
    {
        base.Die();

        stateMachine.ChangeState(deadState);
    }

    public override void EnterDazeState()
    {
        base.EnterDazeState();

        stateMachine.ChangeState(dazeState);
    }

    public void Attack()
    {
        GameObject newArrow = Instantiate(arrowPrefab, arrowLaunchPoint.position, Quaternion.identity);
        Arrow_Controller arrowController = newArrow.GetComponent<Arrow_Controller>();
        arrowController.SetupArrow(this, arrowSpeed, new Vector2(facingDir, 0));

    }

    #region 弹反窗口
    public override void OpenCounterAttackWindow()
    {
    }

    public override void CloseCounterAttackWindow()
    {
    }
    #endregion
}
