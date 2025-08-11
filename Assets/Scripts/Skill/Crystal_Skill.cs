using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CrystalType
{
    Crystal,
    CrystalAegis,
    MultipleCrystal
}

public class Crystal_Skill : Skill
{
    [SerializeField] private GameObject crystalPrefab;
    [SerializeField] private float crystalDuration;
    private Vector2 launchDir;
    public bool crystalUnlocked { get; private set; }
    [Header("水晶")]
    [SerializeField] UI_SkillSlot crystalUnlockButton;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float crystalDamageMultiplier;
    [SerializeField] private float crystalImpactMultiplier;

    public bool crystalAegisUnlocked { get; private set; }
    [Header("水晶护盾")]
    [SerializeField] private UI_SkillSlot crystalAegisUnlockButton;
    [SerializeField] private int crystalAegisAmount;
    [SerializeField] private float orbitRadius;
    [SerializeField] private float rotateSpeed;
    private List<GameObject> currentCrystal;
    [SerializeField] private float crystalAegisDamageMultiplier;
    [SerializeField] private float crystalAegisImpactMultiplier;

    public bool explodeCrystalUnlocked { get; private set; }
    [Header("爆炸水晶")]
    [SerializeField] private UI_SkillSlot explodeCrystalUnlockButton;
    [SerializeField] private float explodeRadius;

    public bool multipleCrystalUnlocked {  get; private set; }
    [Header("多重水晶")]
    [SerializeField] private UI_SkillSlot multipleCrystalUnlockButton;
    [SerializeField] private int multipleCrystalAmount;
    [SerializeField] private float angleRange;
    [SerializeField] private float multipleCrystalDamageMultiplier;
    [SerializeField] private float multipleCrystalImpactMultiplier;
    
    protected override void Start()
    {
        base.Start();

        currentCrystal = new List<GameObject>();

        crystalUnlockButton.unlockSkill.AddListener(UnlockCrystal);
        crystalAegisUnlockButton.unlockSkill.AddListener(UnlockCrystalAegis);
        explodeCrystalUnlockButton.unlockSkill.AddListener(UnlockExplodeCrystal);
        multipleCrystalUnlockButton.unlockSkill.AddListener(UnlockMultipleCrystal);
    }

    protected override void Update()
    {
        base.Update();
    }

    public override bool CanUseSkill(SkillType _skillType = SkillType.None)
    {
        if(_skillType == SkillType.Crystal) return crystalUnlocked && base.CanUseSkill(_skillType);
        if(_skillType == SkillType.CrystalAegis) return crystalAegisUnlocked && base.CanUseSkill(_skillType);
        if(_skillType == SkillType.MultipleCrystals) return multipleCrystalUnlocked && base.CanUseSkill(_skillType);
        return false;
    }

    public override void UseSkill(SkillType _skillType = SkillType.None)
    {
        base.UseSkill();

        
        if(_skillType == SkillType.Crystal)
        {
            CreateCrystal();
        }
        if (_skillType == SkillType.CrystalAegis)
        {
            CreateCrystalAegis();
        }
        if(_skillType == SkillType.MultipleCrystals)
        {
            CreateMultipleCrystal();
        }

    }

    private void CreateCrystal()
    {
        launchDir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - player.transform.position);
        launchDir.Normalize();
        if (launchDir.x * player.facingDir < 0)
        {
            player.Flip();
        }
        GameObject newCrystal = Instantiate(crystalPrefab);
        newCrystal.transform.position = player.transform.position;
        Crystal_Skill_Controller crystalController = newCrystal.GetComponent<Crystal_Skill_Controller>();

        crystalController.SetupCrystal(CrystalType.Crystal, moveSpeed, crystalDuration, launchDir, crystalDamageMultiplier, crystalImpactMultiplier);
        crystalController.SetupExplodeCrystal(explodeCrystalUnlocked, explodeRadius);
    }
    private void CreateCrystalAegis()
    {
        for(int i = currentCrystal.Count - 1; i >= 0; i--)
        {
            Destroy(currentCrystal[i].gameObject);
        }
        currentCrystal.Clear();

        for(int i = 0;i < crystalAegisAmount; i++)
        {
            GameObject newCrystal = Instantiate(crystalPrefab);
            Crystal_Skill_Controller crystalController = newCrystal.GetComponent<Crystal_Skill_Controller>();
            crystalController.SetupCrystal(CrystalType.CrystalAegis, moveSpeed, int.MaxValue, Vector2.zero, crystalAegisDamageMultiplier, crystalAegisImpactMultiplier);
            crystalController.SetupExplodeCrystal(explodeCrystalUnlocked, explodeRadius);
            crystalController.SetupCrystalAegis(orbitRadius, rotateSpeed, 360f / crystalAegisAmount * i);
            currentCrystal.Add(newCrystal);
        }
    }

    private void CreateMultipleCrystal()
    {
        launchDir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - player.transform.position);
        launchDir.Normalize();
        if (launchDir.x * player.facingDir < 0)
        {
            player.Flip();
        }
        for (int i = 0; i < multipleCrystalAmount; i++)
        {
            Invoke("CreateMultipleCrystal_", UnityEngine.Random.Range(0, 0.5f));
        }
    }

    private void CreateMultipleCrystal_()
    {
        float randomAngleOffset = UnityEngine.Random.Range(-angleRange / 2, angleRange / 2);
        Quaternion randomRotation = Quaternion.Euler(0, 0, randomAngleOffset);
        Vector3 newLaunchDir = (randomRotation * launchDir);

        GameObject newCrystal = Instantiate(crystalPrefab);
        newCrystal.transform.position = player.transform.position;
        Crystal_Skill_Controller crystalController = newCrystal.GetComponent<Crystal_Skill_Controller>();

        crystalController.SetupCrystal(CrystalType.Crystal, moveSpeed, crystalDuration, newLaunchDir, multipleCrystalDamageMultiplier, multipleCrystalImpactMultiplier);
        crystalController.SetupExplodeCrystal(explodeCrystalUnlocked, explodeRadius);
    }


    #region 解锁技能
    private void UnlockCrystal() => crystalUnlocked = true;
    private void UnlockCrystalAegis() => crystalAegisUnlocked = true;
    private void UnlockExplodeCrystal() => explodeCrystalUnlocked = true;
    private void UnlockMultipleCrystal() => multipleCrystalUnlocked = true;

    #endregion

}
