using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class Player : Entity
{
    [Header("攻击信息")]
    public Vector2[] attackMovement;

    public bool isBusy { get; private set; }

    [Header("移动信息")]
    public float moveSpeed = 12f;
    public float jumpForce;
    public float swordReturnImpact;
    private float defaultMoveSpeed;
    private float defaultJumpForce;

    public SkillManager skill {  get; private set; }

    #region 状态定义
    public PlayerStateMachine stateMachine { get; private set; }

    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerWallSlideState wallSlideState { get; private set; }
    public PlayerWallJumpState wallJumpState { get; private set; }
    public PlayerPrimaryAttackState primaryAttackState { get; private set; }
    public PlayerBlockState blockState { get; private set; }
    public PlayerThrowSwordState throwSwordState { get; private set; }
    public PlayerCatchSwordState catchSwordState { get; private set; }
    public PlayerBlackholeState blackholeState { get; private set; }
    public PlayerDeadState deadState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new PlayerStateMachine();
        
        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState = new PlayerAirState(this, stateMachine, "Jump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        wallSlideState = new PlayerWallSlideState(this, stateMachine, "WallSlide");
        wallJumpState = new PlayerWallJumpState(this, stateMachine, "Jump");
        primaryAttackState = new PlayerPrimaryAttackState(this, stateMachine, "Attack");
        blockState = new PlayerBlockState(this, stateMachine, "Block");
        throwSwordState = new PlayerThrowSwordState(this, stateMachine, "AimSword");
        catchSwordState = new PlayerCatchSwordState(this, stateMachine, "CatchSword");
        blackholeState = new PlayerBlackholeState(this, stateMachine, "Jump");
        deadState = new PlayerDeadState(this, stateMachine, "Die");

    }  
    

    protected override void Start()
    {
        base.Start();

        skill = SkillManager.instance;

        stateMachine.Initalize(idleState);

        defaultMoveSpeed = moveSpeed;
        defaultJumpForce = jumpForce;
    }

    protected override void Update()
    {
        base.Update();
        if (Time.timeScale == 0) return;

        stateMachine.currentState.Update();
        CheckForDashInput();
    
    }
    
    public bool ReleaseSkill(Ui_GameInerfaceSkillBar _skill)
    {
        SkillType skillType = _skill.skillType;
        if (skillType == SkillType.Throw || skillType == SkillType.Bounce
                    || skillType == SkillType.Pierce || skillType == SkillType.Spin)
        {
            if (skill.sword.CanUseSkill(skillType))
            {
                skill.sword.UseSkill(skillType);
                return true;
            }
        }
        if(skillType == SkillType.Crystal || skillType == SkillType.CrystalAegis || skillType == SkillType.MultipleCrystals)
        {
            if(skill.crystal.CanUseSkill(skillType))
            {
                skill.crystal.UseSkill(skillType);
                return true;
            }
        }
        if(skillType == SkillType.Blockhole)
        {
            if(skill.blackhole.CanUseSkill(skillType))
            {
                return true;
            }
        }
        return false;
    }

    public override void SlowEntityBy(float _slowPercentage, float _slowDuration)
    {
        moveSpeed = moveSpeed * (1 - _slowPercentage);
        jumpForce = jumpForce * (1 - _slowPercentage);
        skill.dash.dashSpeed = skill.dash.dashSpeed * (1 - _slowPercentage);
        anim.speed = anim.speed * (1 - _slowPercentage);

        Invoke("ReturnDefaultSpeed", _slowDuration);
    }

    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();
        moveSpeed = defaultMoveSpeed;
        jumpForce = defaultJumpForce;
        skill.dash.dashSpeed = skill.dash.defaultDashSpeed;
    }

    public IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;

        yield return new WaitForSeconds(_seconds);

        isBusy = false;
    }

    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    private void CheckForDashInput()
    {

        if (Input.GetKeyDown(KeyCode.LeftShift) && SkillManager.instance.dash.CanUseSkill())
        {
            stateMachine.ChangeState(dashState);
        }
    }

    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deadState);
    }

}
