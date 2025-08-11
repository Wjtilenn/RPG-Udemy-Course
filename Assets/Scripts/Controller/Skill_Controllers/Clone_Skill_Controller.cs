using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clone_Skill_Controller : MonoBehaviour
{
    private SpriteRenderer sr;
    private Animator anim;
    [SerializeField] private float colorLoosingSpeed;

    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackCheckRadius = 0.8f;
    [SerializeField] private float attackMultiplier;
    [SerializeField] private float impactMultiplier;
    private Transform closestEnemy;
    // private int facingDir = 1;


    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

    }

    private void Update()
    {
        sr.color = new Color(1, 1, 1, sr.color.a - (Time.deltaTime * colorLoosingSpeed));
        if(sr.color.a < 0)
        {
            Destroy(gameObject);
        }
    }

    public void SetupClone(Transform _newTransform,Vector3 _offset, Transform _closestEnemy, float _attackMultiplier, float _impactMultiplier)
    {
        
        anim.SetInteger("AttackNumer", Random.Range(1, 4));
        

        transform.position = _newTransform.position + _offset;
        closestEnemy = _closestEnemy;
        attackMultiplier = _attackMultiplier;
        impactMultiplier = _impactMultiplier;

        FaceClosestTarget();
    }

    private void AnimationTrigger()
    {
        
    }

    private void AttackTrigger()
    {
        Collider2D[] collders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);

        foreach (var hit in collders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                Enemy enemy = hit.GetComponent<Enemy>();
                EnemyStats target = hit.GetComponent<EnemyStats>();
                PlayerManager.instance.player.stats.DoDamage(target, attackMultiplier, DamageType.Physical);
                enemy.SetupKnockbackDir(transform);
                // TODO: 克隆对敌人造成失衡

            }
        }
    }
    
    private void FaceClosestTarget()
    {
        if(closestEnemy != null)
        {
            if(transform.position.x > closestEnemy.position.x)
            {
                // facingDir = -1;
                transform.Rotate(0, 180, 0);
            }
        }

    }

}
