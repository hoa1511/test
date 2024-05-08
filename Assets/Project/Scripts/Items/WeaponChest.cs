using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponChest : MonoBehaviour, ICanTakeDamage, IHoldingItem, IPlayParticleSystem
{
    [SerializeField] private GameObject brokenChest;
    [SerializeField] private ParticleSystem chestHitParrticleSystem;
    [SerializeField] GameObject weapon;

    public void PlayParticleSystem(ParticleSystem instantiateParticleSystem, Vector3 positionToPlay, Quaternion rotation, Vector3 particleScale)
    {
        ParticleSystem particle = Instantiate(instantiateParticleSystem, positionToPlay, rotation);
        particle.transform.localScale = particleScale;

    }

    public void TakeDamage()
    {
        InstantiateBrokenChest();
        DestroyNormalChest();
        HandleHoldingItem();
    }

    private void DestroyNormalChest()
    {
        transform.GetChild(1).gameObject.SetActive(false);
    }

    private void InstantiateBrokenChest()
    {
        Vector3 particleSystemScale = new Vector3(0.5f,0.5f,0.5f);
        //GameObject brokenChestObject = Instantiate(brokenChest, transform.position, transform.rotation);
        transform.GetChild(0).gameObject.SetActive(true);
        PlayParticleSystem(chestHitParrticleSystem, transform.position, transform.rotation, particleSystemScale);
        foreach(Transform child in transform.GetChild(0).gameObject.transform)
        {
            if(child.TryGetComponent<Rigidbody>(out Rigidbody childRigidBody))
            {
                childRigidBody.AddExplosionForce(1000f,transform.position, 5f);
            }
        }
    }
    public void HandleHoldingItem()
    {
        transform.GetComponent<BoxCollider>().enabled = false;
        transform.GetChild(2).gameObject.SetActive(true);
        transform.GetChild(3).gameObject.SetActive(false);
    }
}
