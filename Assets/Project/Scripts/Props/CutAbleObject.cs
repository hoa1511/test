using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DynamicMeshCutter;

public class CutAbleObject : MonoBehaviour, ICutable, ICanTakeDamage, IPlayParticleSystem, IPlaySound
{
    [SerializeField] private ParticleSystem crateHitParrticleSystem;
    AudioSource audioSource;

    protected virtual void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }
    public virtual void CutObject(CutterBehaviour cutterBehaviour)
    {
        if(gameObject.GetComponent<MeshTarget>())
        {
            cutterBehaviour.Cut(gameObject.GetComponent<MeshTarget>(), cutterBehaviour.gameObject.transform.position, cutterBehaviour.gameObject.transform.forward); 
        }
    }

    public virtual void TakeDamage()
    {
        Vector3 particleSystemScale = new Vector3(0.5f,0.5f,0.5f);
        PlayParticleSystem(crateHitParrticleSystem, transform.position, transform.rotation, particleSystemScale);
    }

    public virtual void PlayParticleSystem(ParticleSystem instantiateParticleSystem, Vector3 positionToPlay, Quaternion rotation, Vector3 particleScale)
    { 
        ParticleSystem particle = Instantiate(instantiateParticleSystem, positionToPlay, rotation);
        particle.transform.localScale = particleScale;
    }

    public virtual void PlaySound(AudioClip sound)
    {
        audioSource.PlayOneShot(sound);
    }
}
