using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossNinja : BossBehaviour
{
    [SerializeField] Transform parent;
    [SerializeField] GameObject cubeLook;
    public override void Update()
    {
        cubeLook.transform.RotateAround(transform.position, Vector3.forward, 100 * Time.deltaTime);
        //cubeLook.transform.position = transform.position - new Vector3(0,2,0);
        if(isDead == false)
        {
            if(countToUseUltimate == numberOfAttackToUseUltimate)
            {
                bossAttackSkill = BossAttackSkill.ULTI_SKILL;
            }

            if(currentCountToAttackPlayer >= countToAttackPlayer)
            {
                transform.LookAt(weaponBoss.player.transform);
                direction = weaponBoss.player.transform.position - transform.position;

                if(weaponBoss.isFly == false)
                {
                    weaponBoss.transform.eulerAngles = new Vector3(0, 0, CalculateAngle(direction, weaponBoss.player.transform));
                }
            }

            else 
            {
                transform.LookAt(cubeLookAt.transform);

                direction = cubeLookAt.transform.position - transform.position;

                if(weaponBoss.isFly == false)
                {
                    weaponBoss.transform.eulerAngles = new Vector3(0, 0, CalculateAngle(direction, cubeLookAt.transform));;
                }
            }

            currentTimeToAttack += Time.deltaTime;

            if(currentTimeToAttack >= timeToAttack)
            {
                if(bossAttackSkill == BossAttackSkill.BASE_ATTACK)
                {
                    BaseAttack();
                }
                if(bossAttackSkill == BossAttackSkill.ULTI_SKILL)
                {
                    if(hasPlayUlti == false)
                    {
                        StartCoroutine(UltiAttack());
                        hasPlayUlti = true;
                    }
                }
            }
        }
    }

    public override IEnumerator UltiAttack()
    {
        SpawnUltiParticleSystem();
        for(int i = 0; i < 10; i++)
        {
            Vector3 dir = (transform.position - cubeLook.transform.position).normalized;
            GameObject ultiTrident = Instantiate(bossUltiObject,transform.position, Quaternion.identity);
            ultiTrident.GetComponent<BossNinjaUlti>().direction = dir;
            
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(1f);
        countToUseUltimate = 0;
        bossAttackSkill = BossAttackSkill.BASE_ATTACK;
        hasPlayUlti = false;
    }
}
