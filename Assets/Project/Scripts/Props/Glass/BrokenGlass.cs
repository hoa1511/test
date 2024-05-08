using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenGlass : MonoBehaviour, ICanTakeDamage, IPlaySound, IPlayParticleSystem
{
    // [SerializeField] private AudioClip brokenGlassSFX;
    // [SerializeField] private ParticleSystem glassHitParrticleSystem;
    
    // private AudioSource audioSource;

    private void Start()
    {
        //audioSource = GetComponent<AudioSource>();
    }
    public void PlayParticleSystem(ParticleSystem instantiateParticleSystem, Vector3 positionToPlay, Quaternion rotation, Vector3 particleScale)
    {
        ParticleSystem particle = Instantiate(instantiateParticleSystem, positionToPlay, rotation);
        particle.transform.localScale = particleScale;
    }

    public void PlaySound(AudioClip sound)
    {
        //audioSource.PlayOneShot(sound);
    }

    public void TakeDamage()
    {
        GetComponent<Collider>().enabled = false;
        //PlaySound(brokenGlassSFX);
        InstantiateBrokenGlass();
    }

    private void InstantiateBrokenGlass()
    {
        for(int i = 0 ; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Rigidbody>().isKinematic = false;
            transform.GetChild(i).GetComponent<Rigidbody>().AddExplosionForce(5, transform.position, 3, 1, ForceMode.Impulse);
        }
    }

    private void DestroyNormalGlass(GameObject go)
    {
        gameObject.SetActive(false);
    }
    
}
