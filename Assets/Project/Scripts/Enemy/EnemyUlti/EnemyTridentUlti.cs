using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTridentUlti : MonoBehaviour
{
   private bool isHit = false;
    private void MoveForWard()
    {
        transform.Translate(0, -0.5f, 0);
    }

    private void Update()
    {
        if(isHit == false)
        {
            MoveForWard();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "wall")
        {
            isHit = true;
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
            Destroy(gameObject, 2f);
        }
        // if(other.gameObject.CompareTag("Player"))
        // {
        //     other.GetComponent<CharacterHolder>().isDead = true;
        // }
    }
}
