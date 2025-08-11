using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    [SerializeField] protected float cooldown;
    protected float cooldownTimer;

    protected Player player;
    protected PlayerStats playerStats;

    protected virtual void Start()
    {
        player = PlayerManager.instance.player;
        playerStats = PlayerManager.instance.player.stats as PlayerStats;
        cooldownTimer = 0;
    }

    protected virtual void Update()
    {
        cooldownTimer -= Time.deltaTime;
    }

    public virtual bool CanUseSkill(SkillType _skillType = SkillType.None)
    {
        if(cooldownTimer < 0) return true;
        Debug.Log("¼¼ÄÜÀäÈ´ÖÐ!");
        return false;
    }

    public virtual void UseSkill(SkillType _skillType = SkillType.None)
    {
        cooldownTimer = cooldown;
    }

    protected virtual Transform FindClosestEnemy(Transform _checkTransform)
    {
        Collider2D[] collider = Physics2D.OverlapCircleAll(_checkTransform.position, 25);

        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (var hit in collider)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                float distanceToEnemy = Vector3.Distance(_checkTransform.position, hit.transform.position);

                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }
        }

        return closestEnemy;
    }
}
