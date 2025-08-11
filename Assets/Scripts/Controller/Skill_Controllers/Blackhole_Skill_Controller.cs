using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackhole_Skill_Controller : MonoBehaviour
{

    private float maxSize;
    private float growSpeed;
    private float shrinkSpeed;
    private float blackholetimer;
    private int cloneAttackAmount;

    private bool canGrow;
    private bool canShrink;
    private bool attackReleased;

    private List<Transform> targets = new List<Transform>();

    public void SetupBlackhole(float _maxSize, float _growSpeed, float _shrinkSpeed, float _blackholeDuration, int _cloneAttackAmount)
    {
        maxSize = _maxSize;
        growSpeed = _growSpeed;
        shrinkSpeed = _shrinkSpeed;
        blackholetimer = _blackholeDuration;
        cloneAttackAmount = _cloneAttackAmount;

        canGrow = true;
        canShrink = false;
        attackReleased = false;
    }

    private void Update()
    {
        blackholetimer -= Time.deltaTime;

        if(blackholetimer < 0)
        {
            blackholetimer = Mathf.Infinity;
            ReleaseCloneAttack();
        }

        if (canGrow && !canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        }

        if (canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), shrinkSpeed * Time.deltaTime);

            if (transform.localScale.x < 0)
            {
                SkillManager.instance.blackhole.SkillFinish();
                Destroy(gameObject);
            }
        }
    }

    public void ReleaseCloneAttack()
    {
        if (attackReleased) return;
        attackReleased = true;
        canGrow = false;
        StartCoroutine(CloneAttack());
    }

    private IEnumerator CloneAttack()
    {
        if(targets.Count > 0)
        {
            for(int i = 0;i < cloneAttackAmount; i++)
            {
                int randomIndex = Random.Range(0, targets.Count);
                float xOffset = Random.Range(0f, 100f) <= 50 ? 1 : -1;
                SkillManager.instance.clone.CreateClone(targets[randomIndex], new Vector3(xOffset, 0, 0));

                yield return new WaitForSeconds(0.1f);
            }
        }
        canShrink = true;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>().FreezeTimer(true);
            targets.Add(collision.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent <Enemy>().FreezeTimer(false);
        }
    }

}
