using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRunState : EnemyState
{
    public EnemyRunState(GameObject gameObject) : base(gameObject) { }
    // Start is called before the first frame update
    public override void Enter()
    {
        animator.SetBool("EnemyRun", true);
    }

    public override void Exit()
    {
        animator.SetBool("EnemyRun", false);
    }
}
