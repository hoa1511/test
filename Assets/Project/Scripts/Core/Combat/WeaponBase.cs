using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour, IDamageable, IPlayParticleSystem, IPlaySound
{
    [SerializeField] public ParticleSystem hitParticle;
    [SerializeField] private AudioClip hitEnemySFX;
    protected AudioSource audioSource;

    protected virtual void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    protected virtual void Update()
    {

    }

    public virtual void HandleDamage(ICanTakeDamage client)
    {   
        client.TakeDamage();   
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<ICanTakeDamage>(out ICanTakeDamage client))
        {
            Vector3 hitParticleScale = new Vector3(0.5f,0.5f,0.5f);
            PlaySound(hitEnemySFX);
            PlayParticleSystem(hitParticle, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation, hitParticleScale);
            HandleDamage(client);
        }
    }

    public void PlayParticleSystem(ParticleSystem instantiateParticleSystem, Vector3 positionToPlay, Quaternion rotation, Vector3 particleScale)
    {
        ParticleSystem hitEffect = Instantiate(instantiateParticleSystem, positionToPlay, rotation);
        hitEffect.transform.localScale = particleScale; 
        Destroy(hitEffect.gameObject, 0.5f);
    }

    public void PlaySound(AudioClip sound)
    {
        if(sound != null)
        {
            audioSource.PlayOneShot(sound);
        } 
    }
}
