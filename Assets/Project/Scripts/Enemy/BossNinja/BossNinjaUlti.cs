using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossNinjaUlti : MonoBehaviour, IDamageable
{
    public Vector3 direction;

    public void HandleDamage(ICanTakeDamage client)
    {
        client.TakeDamage();
    }

    void Start()
    {
        Destroy(gameObject, 2f);
    }
    
    void Update()
    {
        transform.Translate(direction * Time.deltaTime * 5);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<ICanTakeDamage>(out ICanTakeDamage client))
        {
            HandleDamage(client);
        }    
    }

    
}
