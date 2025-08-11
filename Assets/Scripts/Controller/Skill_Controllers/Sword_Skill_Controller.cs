using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Xml;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Sword_Skill_Controller : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D cd;
    private Player player;

    private float returnSpeed = 24f;
    private bool canRotate = true;
    private bool isReturning = false;

    private float attackMultiplier;
    private float impactMultiplier;

    [Header("弹射剑信息")]
    private bool isBouncing;
    private float bounceSpeed = 20;
    private int bounceAmount;
    [SerializeField] private List<Transform> enemyTarget;
    private int targetIndex;

    [Header("穿刺剑信息")]
    private float pierceAmount;

    [Header("旋转剑信息")]
    private bool isSpinning;
    private float maxTravelDistance;
    private float spinDuration;
    private float spinTImer;
    private bool wasStopped;

    private float hitTimer;
    private float hitCooldown;
    private float spinDirection;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CircleCollider2D>();
    }

    private void Start()
    {
        player = PlayerManager.instance.player;
    }

    private void DestoryMe()
    {
        Destroy(gameObject);
    }

    private void Update()
    {
        if (canRotate)
        {
            transform.right = rb.velocity.normalized;
        }

        if (isReturning)
        {
            transform.right = (transform.position - player.transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, player.transform.position) < 1)
            {
                SkillManager.instance.sword.CatchTheSword();
            }
            return;
        }

        if (isBouncing) BounceLogic();
        if (isSpinning) SpinLogic();

    }

    public void SetupSword(Vector2 _dir, float _gravityScale, float _returnSpeed)
    {
        returnSpeed = _returnSpeed;
        rb.velocity = _dir;
        rb.gravityScale = _gravityScale;

        if (isBouncing || isSpinning)
        {
            anim.SetBool("Rotation", true);
        }

        spinDirection = Mathf.Clamp(rb.velocity.x, -1, 1);

        Invoke("DestoryMe", 7f);
    }

    public void SetupSwordMultiplier(float _attackMultiplier, float _impactMultiplier)
    {
        attackMultiplier = _attackMultiplier;
        impactMultiplier = _impactMultiplier;
    }
    public void SetupBounce(bool _isBouncing, int _amountOfBounce, float _bounceSpeed)
    {
        isBouncing = _isBouncing;
        bounceAmount = _amountOfBounce;
        bounceSpeed = _bounceSpeed;

        enemyTarget = new List<Transform>();
    }

    public void SetupPierce(int _pierceAmount)
    {
        pierceAmount = _pierceAmount;
    }

    public void SetupSpin(bool _isSpinning, float _maxTravelDistance, float _spinDuration, float _hitCooldown)
    {
        isSpinning = _isSpinning;
        maxTravelDistance = _maxTravelDistance;
        spinDuration = _spinDuration;
        hitCooldown = _hitCooldown;
    }

    private void BounceLogic()
    {
        if (enemyTarget.Count > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, enemyTarget[targetIndex].position, bounceSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, enemyTarget[targetIndex].position) < 0.1f)
            {
                SwordSkillDamage(enemyTarget[targetIndex].GetComponent<Enemy>());

                targetIndex = (targetIndex + 1) % enemyTarget.Count;
                bounceAmount--;

                if (enemyTarget.Count ==1 || bounceAmount <= 0)
                {
                    isBouncing = false;
                    isReturning = true;
                }
                
            }
        }
    }
    private void SpinLogic()
    {

        if (Vector2.Distance(player.transform.position, transform.position) > maxTravelDistance && !wasStopped)
        {
            StopWhenSpinning();
        }

        if (wasStopped)
        {
            spinTImer -= Time.deltaTime;

             // 旋转剑沿发射方向缓慢移动
             transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + spinDirection, transform.position.y), 1.5f * Time.deltaTime);

            if (spinTImer < 0)
            {
                isReturning = true;
                isSpinning = false;
            }

            hitTimer -= Time.deltaTime;
            if (hitTimer < 0)
            {
                hitTimer = hitCooldown;

                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, cd.radius);

                foreach (var hit in colliders)
                {
                    if (hit.GetComponent<Enemy>() != null)
                    {
                        SwordSkillDamage(hit.GetComponent<Enemy>());
                    }
                }
            }
        }

    }

    private void StopWhenSpinning()
    {
        if (wasStopped) return;
        wasStopped = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        spinTImer = spinDuration;
    }

    public void ReturnSword()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        isReturning = true;
        transform.parent = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isReturning)
        {
            return;
        }

        if(collision.GetComponent<Enemy>() != null)
        {
            if (isBouncing)
            {
                SetupTargetsForBounce(collision);
            }
            else if(!isSpinning)
            {
                Enemy enemy = collision.GetComponent<Enemy>();
                SwordSkillDamage(enemy);
            }
        }

        StuckInto(collision);
    }

    private void SwordSkillDamage(Enemy enemy)
    {
        EnemyStats target = enemy.GetComponent<EnemyStats>();
        // TODO: 投剑积累异常
        PlayerManager.instance.player.stats.DoDamage(target, attackMultiplier, DamageType.Physical);
        enemy.SetupKnockbackDir(transform);
        
        PlayerManager.instance.player.stats.DoDaze(target, impactMultiplier);

    }

    private void SetupTargetsForBounce(Collider2D collision)
    {

        if (enemyTarget.Count <= 0)
        {
            enemyTarget.Add(collision.transform);
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10);

            foreach (var hit in colliders)
            {
                if (hit.GetComponent<Enemy>() != null && hit != collision) 
                {
                    enemyTarget.Add(hit.transform);
                }
            }

        }

    }

    private void StuckInto(Collider2D collision)
    {
        if(pierceAmount > 0 && collision.GetComponent<Enemy>() != null)
        {
            pierceAmount--;
            return;
        }

        if (isSpinning)
        {
            StopWhenSpinning();
            return;
        }

        canRotate = false;

        cd.enabled = false;
        rb.isKinematic = true;
        // 冻结x y轴的移动 实现嵌在敌人或墙上的效果
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        if (isBouncing && enemyTarget.Count > 0)
        {
            return;
        }
        anim.SetBool("Rotation", false);
        transform.parent = collision.transform;
    }
}
