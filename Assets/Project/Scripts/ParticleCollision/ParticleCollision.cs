using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCollision : MonoBehaviour
{
    private ParticleSystem particleSystem;
    void Start()
    {
        particleSystem = this.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnParticleCollision(GameObject other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<CharacterHolder>().TakeDamage();
            Debug.Log("Hit Player");
        }
    }
}
