using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricObstacle : MonoBehaviour
{
    ParticleSystem particleSystemElectric;
    void Start()
    {
        particleSystemElectric = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // private void OnParticleCollision(GameObject other) {
    //     if(other.gameObject.CompareTag("Player"))
    //     {
    //         CharacterHolder ch = other.GetComponent<CharacterHolder>();
    //         ch.TakeDamage();

    //     }
    // }
    // private void OnParticleTrigger()
    // {
    //     ParticleSystem ps = GetComponent<ParticleSystem>();
    //     Component component = ps.trigger.GetCollider(0);

    //     component.GetComponent<CharacterHolder>().TakeDamage();
    // }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            CharacterHolder ch = other.GetComponent<CharacterHolder>();
            ch.TakeDamage();

        }
    }
}
