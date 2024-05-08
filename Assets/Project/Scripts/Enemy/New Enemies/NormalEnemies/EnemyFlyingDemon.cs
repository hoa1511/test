using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlyingDemon : Enemy
{
    [SerializeField] private GameObject mark;


    protected override void Start()
    {
        base.Start();
        mark.SetActive(false);
    }
    protected override void Update()
    {
        if (isDead == false)
        {
            if (detectRangeFOV.canSeePlayer == true)
            {
                if (attackRangeFOV.canSeePlayer == true)
                {
                    mark.SetActive(false);
                    EnemyCombat();
                    return;
                }
                mark.SetActive(true);
                currentDelayTimeAttack += Time.deltaTime;
                if(currentDelayTimeAttack >= delayTimeAttack)
                {
                    FlyTo(detectRangeFOV.targetPos.position);
                }
            }
            else
            {
                mark.SetActive(false);
                PatrolBehaviour();
            }
        }
        
        UpdateTimers();
    }

    protected override void OnCombatFinish()
    {
        base.OnCombatFinish();
    }
}
