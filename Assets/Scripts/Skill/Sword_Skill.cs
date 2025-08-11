using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public enum SwordType
{
    Throw,
    Bounce,
    Pierce,
    Spin
}

public class Sword_Skill : Skill
{

    [Header("投剑技能信息")]
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private float launchForce;
    [SerializeField] private float returnSpeed;
    public GameObject sword;
    private float swordGravity;
    private SwordType swordType;

    public bool throwUnlocked { get; private set; }
    [Header("投掷剑信息")]
    [SerializeField] private UI_SkillSlot throwUnlockButton;
    [SerializeField] private float throwGravity;
    [SerializeField] private float throwDamageMultiplier;
    [SerializeField] private float throwImpactMultiplier;

    public bool bounceUnlocked { get; private set; }
    [Header("弹射剑信息")]
    [SerializeField] private UI_SkillSlot bounceUnlockButton;
    [SerializeField] private int bounceAmount;
    [SerializeField] private float bounceSpeed;
    [SerializeField] private float bounceGravity;
    [SerializeField] private float bounceDamageMultiplier;
    [SerializeField] private float bounceImpactMultiplier;

    public bool pierceUnlocked { get; private set; }
    [Header("穿刺剑信息")]
    [SerializeField] private UI_SkillSlot pierceUnlockButton;
    [SerializeField] private int pierceAmount;
    [SerializeField] private float pierceGravity;
    [SerializeField] private float pierceDamageMultiplier;
    [SerializeField] private float pierceImpactMultiplier;

    public bool spinUnlocked { get; private set; }
    [Header("旋转剑信息")]
    [SerializeField] private UI_SkillSlot spinUnlockButton;
    [SerializeField] private float hitCooldown = 0.35f;
    [SerializeField] private float maxTravelDistance = 7;
    [SerializeField] private float spinDuration = 2;
    [SerializeField] private float spinGravity = 1;
    [SerializeField] private float spinDamageMultiplier;
    [SerializeField] private float spinImpactMultiplier;

    [Header("瞄准辅助线信息")]
    [SerializeField] private int numberOfDots;
    [SerializeField] private float spaceBeetwenDots;
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private Transform dotsParent;
    private GameObject[] dots;
    private Vector2 finalDir;
    private bool aiming;


    protected override void Start()
    {
        base.Start();

        GenereateDots();
        throwUnlockButton.unlockSkill.AddListener(UnlockThrow);
        bounceUnlockButton.unlockSkill.AddListener(UnlockBounce);
        pierceUnlockButton.unlockSkill.AddListener(UnlockPierce);
        spinUnlockButton.unlockSkill.AddListener(UnlockSpin);
 
    }

    private void SetupSword(SwordType _swordType)
    {
        swordType = _swordType;
        if(_swordType == SwordType.Throw)
        {
            swordGravity = throwGravity;
        }
        if(_swordType == SwordType.Bounce)
        {
            swordGravity = bounceGravity;
        }
        if(_swordType == SwordType.Pierce)
        {
            swordGravity = pierceGravity;
        }
        if(_swordType == SwordType.Spin)
        {
            swordGravity = spinGravity;
        }
    }

    protected override void Update()
    {
        base.Update();

        if(aiming) UpdateDotsPosition();
    }

    public override bool CanUseSkill(SkillType _skillType = SkillType.None)
    {
        if (_skillType == SkillType.Throw) return throwUnlocked && base.CanUseSkill(_skillType);
        if (_skillType == SkillType.Bounce) return bounceUnlocked && base.CanUseSkill(_skillType);
        if (_skillType == SkillType.Pierce) return pierceUnlocked && base.CanUseSkill(_skillType);
        if (_skillType == SkillType.Spin) return spinUnlocked && base.CanUseSkill(_skillType);
        return false;
    }

    public override void UseSkill(SkillType _skillType = SkillType.None)
    {
        base.UseSkill(_skillType);
        if(sword == null)
        {
            aiming = true;
            DotsActive(true);
            player.stateMachine.ChangeState(player.throwSwordState);
            if (_skillType == SkillType.Throw) SetupSword(SwordType.Throw);
            if (_skillType == SkillType.Bounce) SetupSword(SwordType.Bounce);
            if (_skillType == SkillType.Pierce) SetupSword(SwordType.Pierce);
            if(_skillType == SkillType.Spin) SetupSword(SwordType.Spin);
        }
        else
        {
            sword.GetComponent<Sword_Skill_Controller>().ReturnSword();
        }

    }

    public void CreateSword()
    {   
        GameObject newSword = Instantiate(swordPrefab, player.transform.position, transform.rotation);
        Sword_Skill_Controller newSwordScript = newSword.GetComponent<Sword_Skill_Controller>();

        switch (swordType)
        {
            case SwordType.Throw:
                newSwordScript.SetupSwordMultiplier(throwDamageMultiplier, throwImpactMultiplier);
                break;
            case SwordType.Bounce:
                newSwordScript.SetupSwordMultiplier(bounceDamageMultiplier, bounceImpactMultiplier);
                newSwordScript.SetupBounce(true, bounceAmount, bounceSpeed);
                break;
            case SwordType.Pierce:
                newSwordScript.SetupSwordMultiplier(pierceDamageMultiplier, pierceImpactMultiplier);
                newSwordScript.SetupPierce(pierceAmount);
                break;
            case SwordType.Spin:
                newSwordScript.SetupSwordMultiplier(spinDamageMultiplier, spinImpactMultiplier);
                newSwordScript.SetupSpin(true, maxTravelDistance, spinDuration, hitCooldown);
                break;
            default:
                break;
        }

        newSwordScript.SetupSword(finalDir, swordGravity, returnSpeed);

        AssignNewSword(newSword);

        DotsActive(false);

    }

    public void AssignNewSword(GameObject _newSword)
    {
        sword = _newSword;
    }

    public void CatchTheSword()
    {
        player.stateMachine.ChangeState(player.catchSwordState);
        Destroy(sword);
    }


    #region 瞄准辅助线
    private void GenereateDots()
    {
        dots = new GameObject[numberOfDots];
        for (int i = 0; i < numberOfDots; i++)
        {
            dots[i] = Instantiate(dotPrefab, player.transform.position, Quaternion.identity, dotsParent);
            dots[i].SetActive(false);
        }
    }

    public Vector2 AimDirection()
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - playerPosition;

        return direction.normalized; 
    }

    public void DotsActive(bool _isActive)
    {
        for (int i = 0;i < numberOfDots;i++)
        {
            dots[i].SetActive(_isActive);
        }
    }

    public void ExitAiming()
    {
        aiming = false;
        finalDir = new Vector2(AimDirection().x * launchForce, AimDirection().y * launchForce);
    }


    private Vector2 GetDotPosition(float t)
    {
        Vector2 positon = (Vector2)player.transform.position + new Vector2(
            AimDirection().normalized.x * launchForce,
            AimDirection().normalized.y * launchForce) * t + 0.5f * (Physics2D.gravity * swordGravity) * (t * t);

        return positon;
    }

    private void UpdateDotsPosition()
    {
        for (int i = 0; i < numberOfDots; i++)
        {
            dots[i].transform.position = GetDotPosition(i * spaceBeetwenDots);
        }
    }
    #endregion

    #region 解锁技能
    private void UnlockThrow() => throwUnlocked = true;
    private void UnlockBounce() => bounceUnlocked = true;
    private void UnlockPierce() => pierceUnlocked = true;
    private void UnlockSpin() => spinUnlocked = true;
    #endregion



}
