using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBabyDragon : Enemy
{
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform spawnArrowPosition;
    [SerializeField] AudioClip shootArrowSFX;
    [SerializeField] private GameObject firePar;
    private bool isFlaming = false;
    private float delayTime;
    private float currentTime;


    protected override void Start()
    {
        base.Start();
        currentTime = 0;
        delayTime = 1.5f;
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
                if(isFlaming == false)
                {
                    Vector3 targetDirection = detectRangeFOV.playerRef.transform.position - transform.position;
                    Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);

                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 4 * Time.deltaTime);
                }

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
                if(isFlaming == false)
                    PatrolBehaviour();
            }
        }

        UpdateTimers();
    }

    public void ThrowingSkill()
    {
        firePar.GetComponent<ParticleSystem>().Play();
        isFlaming = true;
    }

    public void StopThrowingSkill()
    {
        firePar.GetComponent<ParticleSystem>().Stop();
        isFlaming = false;
    }

    protected override void OnCombatFinish()
    {
    }
}
