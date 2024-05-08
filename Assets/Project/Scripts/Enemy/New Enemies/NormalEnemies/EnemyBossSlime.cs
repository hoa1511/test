using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossSlime : Enemy
{
    protected override void Start()
    {
        base.Start();
    }
    protected override void Update()
    {
        if (isDead == false)
        {
            if (detectRangeFOV.canSeePlayer == true)
            {
                if (attackRangeFOV.canSeePlayer == true)
                {
                    EnemyCombat();
                    return;
                }
                currentDelayTimeAttack += Time.deltaTime;
                if(currentDelayTimeAttack >= delayTimeAttack)
                {
                    RunTo(detectRangeFOV.targetPos.position);
                }
            }
            else
            {
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
