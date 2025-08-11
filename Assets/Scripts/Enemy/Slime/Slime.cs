using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public enum SlimeType
{
    Big,
    Medium,
    Small
}
public class Slime : Enemy
{
    #region ×´Ì¬¶¨Òå

    public SlimeIdleState idleState { get; private set; }
    public SlimeMoveState moveState { get; private set; }
    public SlimeBattleState battleState { get; private set; }
    public SlimeAttackState attackState { get; private set; }
    public SlimeDeadState deadState { get; private set; }
    public SlimeDazeState dazeState { get; private set; }
    #endregion

    [Header("Ê·À³Ä·ËÀÍö·ÖÁÑ")]
    [SerializeField] private SlimeType slimeType;
    [SerializeField] private GameObject mediumSlimePrefab;
    [SerializeField] private GameObject smallSlimePrefab;
    [SerializeField] int bigToMediumCount;
    [SerializeField] int mediumToSmallCount;
    

    protected override void Awake()
    {
        base.Awake();

        idleState = new SlimeIdleState(this, stateMachine, "Idle", this);
        moveState = new SlimeMoveState(this, stateMachine, "Move", this);
        battleState = new SlimeBattleState(this, stateMachine, "Move", this);
        attackState = new SlimeAttackState(this, stateMachine, "Attack", this);
        deadState = new SlimeDeadState(this, stateMachine, "Die", this);
        dazeState = new SlimeDazeState(this, stateMachine, "Daze", this);
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
        if (slimeType == SlimeType.Big)
        {
            for (int i = 0; i < bigToMediumCount; i++)
            {
                Vector3 xOffset = new Vector3(Random.Range(-1.0f, 1.0f), 0);
                Instantiate(mediumSlimePrefab, transform.position + xOffset, Quaternion.identity);
            }
        }
        else if (slimeType == SlimeType.Medium)
        {
            for (int i = 0; i < mediumToSmallCount; i++)
            {
                Vector3 xOffset = new Vector3(Random.Range(-1.0f, 1.0f), 0);
                Instantiate(smallSlimePrefab, transform.position + xOffset, Quaternion.identity);
            }
        }
    }

    public override void EnterDazeState()
    {
        base.EnterDazeState();
    }
}
