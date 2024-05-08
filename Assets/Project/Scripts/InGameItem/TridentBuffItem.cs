using UnityEngine;
using System.Collections;

public class TridentBuffItem : InGameItem
{
    [Header("Buff Duration")]
        [SerializeField] private float duration = 2f;
        
    [Header("Buff Multiplier")]
        [SerializeField] private float multiplier = 2;

    protected override void HandlePickupItem(GameObject gameObjectToApply)
    {
        HideItem();
        StartCoroutine(StartBuffTrident(gameObjectToApply));
    }

    private IEnumerator StartBuffTrident(GameObject gameObjectToApply)
    {
        gameObjectToApply.GetComponent<Collider>().enabled = false;
        gameObjectToApply.GetComponent<Weapon>().Multiplier *= multiplier;

        yield return new WaitForSecondsRealtime(duration);
        
        gameObjectToApply.GetComponent<Collider>().enabled = true;
        gameObjectToApply.GetComponent<Weapon>().Multiplier /= multiplier;
    }

    private void HideItem()
    {
        // GetComponent<MeshRenderer>().enabled = false;
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
    }
}
