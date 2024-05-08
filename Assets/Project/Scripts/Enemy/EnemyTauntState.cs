using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTauntState : EnemyState
{
    public EnemyTauntState(GameObject gameObject) : base(gameObject){}

    public override void Enter()
    {
        animator.SetBool("EnemyTaunt", true);
    }

    public override void Exit()
    {
        animator.SetBool("EnemyTaunt", false);
    }
}
