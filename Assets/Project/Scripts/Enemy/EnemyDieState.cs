using UnityEngine;

public class EnemyDieState : EnemyState
{
    public EnemyDieState(GameObject gameObject) : base(gameObject){}

    public override void Enter()
    {
        animator.SetBool("EnemyDie", true);
    }

    public override void Exit()
    {
        animator.SetBool("EnemyDie", false);
    }
}

