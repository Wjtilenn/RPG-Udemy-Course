using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherAnimationTrigger : MonoBehaviour
{
    private Archer enemy;

    private void Start()
    {
        enemy = GetComponentInParent<Archer>();
    }

    private void AnimationTrigger()
    {
        enemy.AnimationFinishTrigger();
    }

    private void AttackTrigger()
    {
        enemy.Attack();
    }
}
