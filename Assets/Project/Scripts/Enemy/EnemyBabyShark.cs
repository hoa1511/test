using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBabyShark : Enemy
{
    [SerializeField] private AudioClip enemyBabySharkFightSFX;
    FieldOfView enemyFieldOfView;
    
    protected override void Start()
    {
        enemyFieldOfView = this.GetComponent<FieldOfView>();
        
        base.Start();
    }

    protected override void Update()
    {
        if(isDead == false)
        {
            PatrolBehaviour();
        }
        
        if(enemyFieldOfView.canSeePlayer == true)
        {
            EnemyCombat();
        }
        
        base.Update();
    }
}
