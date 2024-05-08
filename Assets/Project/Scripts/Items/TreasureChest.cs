using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChest : MonoBehaviour, ICanTakeDamage, IPlayParticleSystem, IHoldingItem, IPlaySound
{
    [SerializeField] private GameObject brokenChest;
    [SerializeField] private AudioClip brokenChestSFX;
    [SerializeField] private ParticleSystem chestHitParrticleSystem;
    
    [SerializeField] private int numberOfCoinHolding = 10;

    private SpawnCoinFactory spawnCoinFactory;
    private AudioSource audioSource;
    

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        spawnCoinFactory = SpawnCoinFactory.Instance;
    }

    public void PlayParticleSystem(ParticleSystem instantiateParticleSystem, Vector3 positionToPlay, Quaternion rotation, Vector3 particleScale)
    {
        ParticleSystem particle = Instantiate(instantiateParticleSystem, positionToPlay, rotation);
        particle.transform.localScale = particleScale;
    }

    public void TakeDamage()
    {
        PlaySound(brokenChestSFX);
        InstantiateBrokenChest();
        DestroyNormalChest();
        HandleHoldingItem();
    }

    private void DestroyNormalChest()
    {
        QuestManager.Instance.DailyQuestController.DoDailyQuest(QuestType.Destroy);
        transform.GetChild(1).gameObject.SetActive(false);
    }

    private void InstantiateBrokenChest()
    {
        Vector3 particleSystemScale = new Vector3(0.5f,0.5f,0.5f);

        transform.GetChild(0).gameObject.SetActive(true);

        PlayParticleSystem(chestHitParrticleSystem, transform.position, transform.rotation, particleSystemScale);
        // foreach(Transform child in transform.GetChild(0).gameObject.transform)
        // {
        //     if(child.TryGetComponent<Rigidbody>(out Rigidbody childRigidBody))
        //     {
        //         childRigidBody.AddExplosionForce(1000f,transform.position, 5f);
        //     }
        // }
    }

    private IEnumerator SpawnDropCoin(Vector3 spawnItemPosition, Vector3 positionToJumpOut)
    {
        spawnCoinFactory.GetSpawnItem(spawnItemPosition, positionToJumpOut);
        yield return new WaitForSeconds(0);
    }

    public void HandleHoldingItem()
    {
        int bonusCoinFromSkin = (int)CharacterStatController.Instance.GetCurrentSkinStat(PassiveStats.PercentGoldPerChest);
        int numberOfCoinToSpawn = numberOfCoinHolding + numberOfCoinHolding*bonusCoinFromSkin/100;
        transform.GetComponent<BoxCollider>().enabled = false;
        for(int i = 0; i < numberOfCoinToSpawn; i++)
        {
            float randomXPos = Random.Range(-1f, 1f);
            float randomZPos = Random.Range(-0.4f, 0.4f);
            StartCoroutine(SpawnDropCoin(transform.position,new Vector3(transform.position.x + randomXPos, transform.position.y - 0.3f, transform.position.z + randomZPos)));
        }
    }

    public void PlaySound(AudioClip sound)
    {
        audioSource.PlayOneShot(sound);
    }
}

