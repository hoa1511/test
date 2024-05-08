using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySenseState : EnemyState
{
    public EnemySenseState(GameObject gameObject) : base(gameObject) { }
    // Start is called before the first frame update
    public override void Enter()
    {
        animator.SetBool("EnemySense", true);
    }

    public override void Exit()
    {
        animator.SetBool("EnemySense", false);
    }
}
