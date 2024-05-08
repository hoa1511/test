using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFakeChest : Enemy, IHoldingItem
{
    [Header("Number Of Coin Holding")]
        [SerializeField] private int numberOfCoinHolding;

    public void HandleHoldingItem()
    {
        for(int i = 0; i < numberOfCoinHolding; i++)
        {
            float randomXPos = Random.Range(-1f, 1f);
            float randomZPos = Random.Range(-0.4f, 0.4f);
            StartCoroutine(SpawnDropCoin(transform.position,new Vector3(transform.position.x + randomXPos, transform.position.y, transform.position.z + randomZPos)));
        }
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        if(isDead == false)
        {

            if(detectRangeFOV.canSeePlayer == true)
            {
                EnemyCombat(); 
            }
            else
            {
               EnemyIdle();
            }
        }   
    }

    private IEnumerator SpawnDropCoin(Vector3 spawnItemPosition, Vector3 positionToJumpOut)
    {
        SpawnCoinFactory.Instance.GetSpawnItem(spawnItemPosition, positionToJumpOut);
        yield return new WaitForSeconds(0.2f);
    }

    protected override void OnDieFinish()
    {
        // base.OnDieFinish();
        HandleHoldingItem();
    }


    protected override void EnemyCombat()
    {
        StopEnemy();
        Vector3 targetDirection = new Vector3(detectRangeFOV.playerRef.transform.position.x, transform.position.y, detectRangeFOV.playerRef.transform.position.z) - transform.position;
       
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 4 * Time.deltaTime);
        
        enemyCurrentState.Exit();
        enemyCurrentState = new EnemyCombatState(gameObject);
        enemyCurrentState.Enter();
    }
}
