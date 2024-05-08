using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3ron : Enemy
{
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform spawnArrowPosition;
    [SerializeField] AudioClip shootArrowSFX;
    
    private float delayTime;
    private float currentTime;
    protected override void Start()
    {
        base.Start();
        currentTime = 0;
        delayTime = 2.5f;
    }

    protected override void Update()
    {
        if(isDead == false)
        {
            currentTime += Time.deltaTime;
            if(currentTime > delayTime)
            {
                EnemyCombat();
                currentTime = 0;
            }
            else
            {
                EnemyIdle();
            }
        }   
    }

    public void ThrowingSkill()
    {
        PlaySound(shootArrowSFX);
        GameObject arrow = Instantiate(arrowPrefab, spawnArrowPosition.position, spawnArrowPosition.transform.rotation);
        Destroy(arrow, 1);
    }
}
