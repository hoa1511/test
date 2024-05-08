using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBarrel : MonoBehaviour, IExplosionable, ICanTakeDamage, IDamageable, IPlayParticleSystem
{
    [SerializeField] private ParticleSystem explosionParricleSystem;
    [SerializeField] private float explosionRadius;
    public LayerMask explosionTarget;
    Collider[] objectsInRange = null;

    public void Explosion()
    { 
        if(objectsInRange != null)
        {
            if(objectsInRange.Length > 0)
            {
                foreach (Collider col in objectsInRange)
                {
                    if(col.TryGetComponent<ICanTakeDamage>(out ICanTakeDamage client))
                    {
                        HandleDamage(client);
                    }
                }
            }
        } 
        else
        {
            //DONOTHING
        }
    }

    IEnumerator Check()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            ExplosionRadiusCheck();
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
       
    private void ExplosionRadiusCheck()
    {
        objectsInRange = Physics.OverlapSphere(transform.position, explosionRadius, explosionTarget);
    }

    public void HandleDamage(ICanTakeDamage client)
    {
        client.TakeDamage();
    }

    public void TakeDamage()
    {
        Vector3 particleSystemScale = new Vector3(0.5f,0.5f,0.5f);

        Explosion();
        PlayParticleSystem(explosionParricleSystem, transform.position, transform.rotation, particleSystemScale);
        DisableNormalBarrel();
        InstantiateBrokenBarrel();
        GetComponent<Collider>().enabled = false; 
    }

    private void InstantiateBrokenBarrel()
    {
        transform.GetChild(1).gameObject.SetActive(true);
    }

    private void DisableNormalBarrel()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        if(transform.childCount > 4)
        {
            transform.GetChild(3).gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        StartCoroutine(Check());
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("wall"))
        {
            TakeDamage();
        }
    }

    public void PlayParticleSystem(ParticleSystem instantiateParticleSystem, Vector3 positionToPlay, Quaternion rotation, Vector3 particleScale)
    {
        ParticleSystem particle = Instantiate(instantiateParticleSystem, positionToPlay, rotation);
        particle.transform.localScale = particleScale;
        Destroy(particle.gameObject, 1f);
    }
}
