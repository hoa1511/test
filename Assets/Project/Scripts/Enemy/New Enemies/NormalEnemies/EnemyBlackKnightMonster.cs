using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBlackKnightMonster : Enemy
{
    // [SerializeField] FieldOfView attackRangeFieldOfView;
    // [SerializeField] FieldOfView detectFieldOfView;
    [SerializeField] GameObject weaponTrail;
    [SerializeField] private GameObject mark;


    protected override void Start()
    {
        base.Start();
        mark.SetActive(false);
    }
    protected override void Update()
    {
        if (Time.timeScale == 1 && animatorUpdateMode.Equals(AnimatorUpdateMode.UnscaledTime))
        {
            animatorUpdateMode = AnimatorUpdateMode.Normal;
            enemyAnimator.updateMode = animatorUpdateMode;
            EnemyIdle();
            return;
        }
        if (isDead == false)
        {
            if (detectRangeFOV.canSeePlayer == true)
            {
                if (attackRangeFOV.canSeePlayer == true)
                {
                    mark.SetActive(false);
                    weaponTrail.SetActive(true);
                    EnemyCombat();
                    return;
                }
                mark.SetActive(true);
                currentDelayTimeAttack += Time.deltaTime;
                if (currentDelayTimeAttack >= delayTimeAttack)
                {
                    RunTo(detectRangeFOV.targetPos.position);
                }
            }
            else
            {
                weaponTrail.SetActive(false);
                mark.SetActive(false);
                PatrolBehaviour();
            }
        }

        UpdateTimers();
    }
}
