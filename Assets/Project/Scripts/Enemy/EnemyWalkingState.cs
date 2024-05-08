using UnityEngine;

public class EnemyWalkingState : EnemyState
{
    public EnemyWalkingState(GameObject gameObject) : base(gameObject){}

    public override void Enter()
    {
        animator.SetBool("EnemyWalking", true);
    }

    public override void Exit()
    {
        animator.SetBool("EnemyWalking", false);
    }
}
