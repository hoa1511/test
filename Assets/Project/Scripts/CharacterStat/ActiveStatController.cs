using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ActiveStatController : MonoBehaviour
{
    [Header("Thunder Skill")]
    [SerializeField] private ParticleSystem thunderParticleSystem;

    [Header("Roar Skill")]
    [SerializeField] private ParticleSystem roarParticleSystem;

    [Header("Trident Rain")]
    [SerializeField] private GameObject tridentRainGameObject;
    [SerializeField] private GameObject spawnPosCubeGameObject;

    public static ActiveStatController Instance;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }    
        else
        {
            Destroy(this);
        }
    }

    public void PlaySkinSkill(ActiveStats stats)
    {
        switch(stats)
        {
            case ActiveStats.Thunder:
                UseThunderSkill();
            break;
            case ActiveStats.TridentRain:
                UseTridentRain();
            break;
            case ActiveStats.Roar:
                UseRoarSkill();
            break;
        }
    }

#region  "Thunder Skill"
    public void UseThunderSkill()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for(int i = 0; i < enemies.Length; i++)
        {
            ParticleSystem thunderParticle = Instantiate(thunderParticleSystem, enemies[i].transform.position, Quaternion.Euler(90,0,0));
            enemies[i].GetComponent<Enemy>().TakeDamage();
            StatusTextController.Instance.InstantiateStatusText("Shocked", StatusTextController.Instance.electricShockSprite, enemies[i].transform.position, Color.yellow, 0.3f);
            Destroy(thunderParticle.gameObject, 0.5f);
        }
    }
#endregion

#region  "Trident Rain"
    public void UseTridentRain()
    {
        Vector3 spawnPosCubePosition = new Vector3(-2,6,0);
        GameObject spawnPosCube = Instantiate(spawnPosCubeGameObject, spawnPosCubePosition, Quaternion.identity);
        spawnPosCube.transform.DOMoveX(2f, 1);
        StartCoroutine(SpawnTridentUlti(spawnPosCube));
    }
    private IEnumerator SpawnTridentUlti(GameObject positionToSpawnGameObject)
    {
        for(int i = 0; i < 10; i++)
        {
            GameObject ulti = Instantiate(tridentRainGameObject, positionToSpawnGameObject.transform.position, Quaternion.Euler(0,0,0));
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(0.1f);
        Destroy(positionToSpawnGameObject);
    }
#endregion

#region "Roar"
    public void UseRoarSkill()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        ParticleSystem roarParticle = Instantiate(roarParticleSystem, player.transform.position, Quaternion.Euler(90,0,0));
        for(int i = 0; i < enemies.Length; i++)
        {
            enemies[i].GetComponent<Enemy>().GetComponent<Animator>().enabled = false;
        }
        Destroy(roarParticle.gameObject, 1f);
    }
#endregion
}
