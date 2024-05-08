using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEvilMerge : Enemy
{
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform spawnArrowPosition;
    [SerializeField] AudioClip shootArrowSFX;
    
    private float delayTime;
    private float currentTime;
    FieldOfView enemyFieldOfView;

    protected override void Start()
    {
        base.Start();
        currentTime = 0;
        delayTime = 2.5f;
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
        if(isDead == false)
        {
            if(detectRangeFOV.canSeePlayer == true)
            {
                Vector3 targetDirection = new Vector3(detectRangeFOV.playerRef.transform.position.x, transform.position.y, detectRangeFOV.playerRef.transform.position.z) - transform.position;
       
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 4 * Time.deltaTime);

                currentTime += Time.deltaTime;
                if(currentTime > delayTime)
                {
                    EnemyCombat();
                    currentTime = 0;
                }
                else
                {
                    EnemyIdle();
                    isStopped = false;
                }
            }
            else
            {
                isStopped = false;
                PatrolBehaviour();
            }
        }

        UpdateTimers();
    }

    public void ThrowingSkill()
    {
        PlaySound(shootArrowSFX);
        GameObject arrow = Instantiate(arrowPrefab, spawnArrowPosition.position, spawnArrowPosition.transform.rotation);
        arrow.GetComponent<EvilMerge>().spawnPos = spawnArrowPosition.position;
        Destroy(arrow, 3);
    }
}
