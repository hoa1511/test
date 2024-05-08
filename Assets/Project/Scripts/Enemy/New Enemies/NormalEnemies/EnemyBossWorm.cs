using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossWorm : Enemy
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void OnCombatFinish()
    {
        base.OnCombatFinish();
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
