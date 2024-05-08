
using UnityEngine;

public abstract class EnemyState
{
    protected GameObject gameObj;
    protected Animator animator;

    public EnemyState(GameObject gameObject)
    {
        this.gameObj = gameObject;
        animator = gameObject.GetComponent<Animator>();
    }

    public abstract void Enter();
    public abstract void Exit();
}