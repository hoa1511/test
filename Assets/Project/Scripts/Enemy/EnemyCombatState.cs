
using UnityEngine;

public class EnemyCombatState : EnemyState
{
    public EnemyCombatState(GameObject gameObject) : base(gameObject){}

    public override void Enter()
    {
        animator.SetBool("EnemyCombat", true);
    }

    public override void Exit()
    {
        animator.SetBool("EnemyCombat", false);
    }
}