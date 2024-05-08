using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyFireMan : Enemy
{
    [SerializeField] ParticleSystem absorbParticle;
    [SerializeField] Transform releasePostion;
    [SerializeField] ParticleSystem firePowerParticle;
    [SerializeField] Transform firemanUltiSpawnPos;
    [SerializeField] GameObject[] fireManUltiMeteorite;
    [SerializeField] Transform meteoriteTarget;


    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        if(isDead == false)
        {
            PatrolBehaviour();
        }
        
        base.Update();
    }

   protected override void PatrolBehaviour()
    {
        Vector3 nextPosition = enemyPosition;

        if (patrolPath != null)
        {
            if (AtWaypoint())
            {
                EnemyIdle();
                timeSinceArrivedAtWaypoint = 0;
                CycleWaypoint();
            }

            nextPosition = GetCurrentWaypoint();
        }
        
        if(timeSinceArrivedAtWaypoint > waypointDwellTime)
        {
            MoveTo(nextPosition);
        } 
    }

    public void AbsorbPower()
    {
        Vector3 absorbScale = new Vector3(20,20,20);
        absorbParticle.gameObject.SetActive(true);
        absorbParticle.transform.localScale = new Vector3(0,0,0);
        absorbParticle.transform.DOScale(absorbScale, 1f).OnComplete(()=>{
            absorbParticle.transform.DOScale(Vector3.zero, 0.1f).OnComplete(()=>{
                absorbParticle.gameObject.SetActive(false);
            });
        });
    }

    public void ReleasePower()
    {
        if(isDead == false)
        {
            Vector3 releaseScale = new Vector3(20,20,20);

            ParticleSystem explodeParticle = Instantiate(firePowerParticle);

            explodeParticle.transform.position = new Vector3(releasePostion.transform.position.x, releasePostion.transform.position.y + 0.5f, releasePostion.transform.position.z);
            explodeParticle.transform.localScale = new Vector3(0.5f,0.5f,0.5f);
        }
    }

    private void FireManUlti()
    {
        Vector3 meteoriteDirection;
        if(player.GetComponent<CharacterHolder>().isDead == false)
        {
            meteoriteDirection = player.transform.position - firemanUltiSpawnPos.position;
        }

        else
        {
            meteoriteDirection = meteoriteTarget.position - firemanUltiSpawnPos.position;
        }
        int randomIndex = Random.Range(0, fireManUltiMeteorite.Length);
        
        GameObject fireManMeteorite =  Instantiate(fireManUltiMeteorite[randomIndex], firemanUltiSpawnPos);

        fireManMeteorite.GetComponent<Rigidbody>().AddForce(meteoriteDirection.normalized * 10, ForceMode.Impulse);
    }
}
