using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour,IDamageable
{
    public virtual void HandleDamage(ICanTakeDamage client)
    {
        client.TakeDamage();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<ICanTakeDamage>(out ICanTakeDamage client))
        {
            HandleDamage(client);
        }    
    }
}
