using UnityEngine;

public class InGameItem : MonoBehaviour
{
    public string itemName;
    public ParticleSystem hitEffect;
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Trident"))
        {
            ParticleSystem hitParticleSystem = Instantiate(hitEffect, transform.position, Quaternion.identity);
            hitParticleSystem.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            
            HandlePickupItem(other.gameObject);
        }
    }
    protected virtual void HandlePickupItem(GameObject gameObjectToApply)
    {

    }
}
