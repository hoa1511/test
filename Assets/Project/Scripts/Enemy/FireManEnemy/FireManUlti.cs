using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireManUlti : MonoBehaviour, IDamageable
{
    [SerializeField] ParticleSystem fireBallExplosionParticle;
    private bool hasExplode = false;

    private void Update()
    {
        transform.Rotate(new Vector3(10,0,0));
    }
    
    private void OnTriggerEnter(Collider other)
    {
        transform.GetChild(1).gameObject.SetActive(false);
        Destroy(gameObject,0.5f); 
        if(hasExplode == false)
        {
            if(other.gameObject.TryGetComponent<ICanTakeDamage>(out ICanTakeDamage client))
            {
                HandleDamage(client);
            } 
            hasExplode = true;   
            
            Instantiate(fireBallExplosionParticle, transform.position, Quaternion.identity);

            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;

            transform.GetChild(0).gameObject.SetActive(false);

            
            
        }
        
    }

    private void OnCollisionEnter(Collision other)
    {
        transform.GetChild(1).gameObject.SetActive(false);
        Destroy(gameObject,0.5f);
        if(hasExplode == false)
        {
            Instantiate(fireBallExplosionParticle, transform.position, Quaternion.identity);

            

            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;

            transform.GetChild(0).gameObject.SetActive(false);

            hasExplode = true;
        }
        
    }

    public void HandleDamage(ICanTakeDamage client)
    {
        client.TakeDamage();
    }
}
