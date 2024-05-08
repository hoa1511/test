using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSupremeMan : BossBehaviour 
{
    [SerializeField] List<Transform> supremenmanUltiSpawnPos = new List<Transform>();
    [SerializeField] GameObject[] suprememanUlti;
    [SerializeField] Transform defaultMeteoriteTarget;

    public override void Update()
    {
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
        CharacterHolder player;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterHolder>();

       
            for(int i = 0; i < supremenmanUltiSpawnPos.Count; i++)
            {
                Vector3 direction;
                int randomIndex = Random.Range(0, suprememanUlti.Length);
                if(player.isDead == false)
                {
                    direction = player.transform.position - supremenmanUltiSpawnPos[i].position;   
                }

                else
                {
                    direction = defaultMeteoriteTarget.position - supremenmanUltiSpawnPos[i].position;
                }
                GameObject suprememanMeteorite = Instantiate(suprememanUlti[randomIndex], supremenmanUltiSpawnPos[i]);
                suprememanMeteorite.GetComponent<Rigidbody>().AddForce(direction.normalized * 10, ForceMode.Impulse);
                yield return new WaitForSeconds(1f);
            }
        
        yield return new WaitForSeconds(1f);
        countToUseUltimate = 0;
        bossAttackSkill = BossAttackSkill.BASE_ATTACK;
        hasPlayUlti = false;
    }
}
