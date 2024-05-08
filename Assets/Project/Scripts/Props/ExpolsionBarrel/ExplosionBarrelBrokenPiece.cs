using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBarrelBrokenPiece : MonoBehaviour, IDamageable
{
    public void HandleDamage(ICanTakeDamage client)
    {
        client.TakeDamage();
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.TryGetComponent<ICanTakeDamage>(out ICanTakeDamage client))
        {
            HandleDamage(client);
        }
    }
}
