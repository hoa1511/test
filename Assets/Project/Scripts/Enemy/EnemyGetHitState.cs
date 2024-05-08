using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGetHitState : EnemyState
{
    public EnemyGetHitState(GameObject gameObject) : base(gameObject){}

    public override void Enter()
    {
        animator.SetBool("EnemyGetHit", true);
    }

    public override void Exit()
    {
        animator.SetBool("EnemyGetHit", false);
    }
}
