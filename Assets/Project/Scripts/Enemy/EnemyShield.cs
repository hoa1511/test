using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShield : Enemy
{
    [SerializeField] AudioClip swordSlashSound;
    [SerializeField] GameObject shield;
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

            if(enemyFieldOfView.canSeePlayer == true)
            {
                EnemyCombat(); 
            }
        }   
        base.Update();
    }

    public void PlaySwordSlashSound()
    {
        PlaySound(swordSlashSound);
    }

    public override void TakeDamage()
    {   
       base.TakeDamage();
       if(shield != null)
       {
            shield.gameObject.GetComponent<Collider>().enabled = false;
       }
    }
}
