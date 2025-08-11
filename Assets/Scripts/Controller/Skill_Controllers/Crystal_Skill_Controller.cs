using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal_Skill_Controller : MonoBehaviour
{
    private Animator anim => GetComponent<Animator>();
    private Rigidbody2D rb => GetComponent<Rigidbody2D>();
    private CircleCollider2D cd => GetComponent<CircleCollider2D>();
    private SpriteRenderer sr => GetComponent<SpriteRenderer>();

    private CrystalType myType;
    private float moveSpeed;
    private float crystalTimer = Mathf.Infinity;
    private Vector2 launchDir;

    [Header("水晶攻击信息")]
    private float attackMultiplier;
    private float impactMultiplier;

    [Header("水晶护盾")]
    private float orbitRadius;
    private float rotateSpeed;
    private float currentDeg;

    [Header("爆炸水晶")]
    private bool canExplode;
    private float explodeRadius;

    public void SetupCrystal(CrystalType _type, float _moveSpeed, float _crystalDuration, Vector2 _launchDir, float _attackMultiplier, float _impactMultiplier)
    {
        myType = _type;
        moveSpeed = _moveSpeed;
        launchDir = _launchDir;
        crystalTimer = _crystalDuration;
        attackMultiplier = _attackMultiplier;
        impactMultiplier = _impactMultiplier;

        if (myType == CrystalType.Crystal || myType == CrystalType.MultipleCrystal) CrystalLogic();
    }
    
    public void SetupCrystalAegis(float _orbitRadius, float _rotateSpeed, float _initAngle)
    {
        orbitRadius = _orbitRadius;
        rotateSpeed = _rotateSpeed;
        currentDeg = _initAngle;
    }
    public void SetupExplodeCrystal(bool _canExplode, float _explodeRadius)
    {
        canExplode = _canExplode;
        explodeRadius = _explodeRadius;
    }
    private void Update()
    {
        if (myType == CrystalType.CrystalAegis) CrystalAegisLogic();
        
        crystalTimer -= Time.deltaTime;
        if(crystalTimer < 0) FinishCrystal();


    }

    private void CrystalLogic()
    {
        transform.right = new Vector2(-launchDir.y, launchDir.x);
        rb.velocity = launchDir * moveSpeed;
    }
    private void CrystalAegisLogic()
    {
        currentDeg += rotateSpeed * Time.deltaTime;
        while (currentDeg >= 360) currentDeg -= 360;

        sr.sortingOrder = currentDeg < 180f ? -1 : 1;

        float currentRad = currentDeg * Mathf.Deg2Rad;
        Vector2 playerPosition = PlayerManager.instance.player.transform.position;
        transform.position = new Vector2(playerPosition.x + orbitRadius * Mathf.Cos(currentRad), playerPosition.y - 0.2f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null) return;

        rb.velocity = Vector2.zero;
        if (collision.GetComponent<Enemy>() != null)
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            EnemyStats target = collision.GetComponent<EnemyStats>();
            if (canExplode)
            {
                anim.SetTrigger("Explode");
            }
            else
            {
                PlayerManager.instance.player.stats.DoDamage(target, attackMultiplier, DamageType.Ice);
                enemy.SetupKnockbackDir(transform);
                SelfDestroy();
            }
            return;
        }

        FinishCrystal();
    }

    public void FinishCrystal()
    {
        if (canExplode)
        {
            anim.SetTrigger("Explode");
        }
        else
        {
            SelfDestroy();
        }
    }

    public void AnimationExplodeEvent()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explodeRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                // TODO: 水晶爆炸对敌人造成伤害
                //PlayerManager.instance.player.stats.DoMagicalDamage(hit.GetComponent<CharacterStats>());

            }
        }
    }

    public void GrowSize()
    {
        transform.localScale = Vector3.one * 3;
        cd.enabled = false;
        
    }
    public void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
