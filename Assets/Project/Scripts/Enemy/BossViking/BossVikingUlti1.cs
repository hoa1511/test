using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossVikingUlti1 : MonoBehaviour, IDamageable
{
    void Update()
    {
        transform.Rotate(new Vector3(0,0,20));
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.TryGetComponent<ICanTakeDamage>(out ICanTakeDamage client))
        {
            HandleDamage(client);
        }
    }
    
    public void HandleDamage(ICanTakeDamage client)
    {
        client.TakeDamage();
    }
}
