using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectShield : MonoBehaviour, IReflectDamage
{
    [SerializeField] private Transform oppositeDirection;
    private Vector3 currentPosition;
    private CharacterHolder character;

    private void Start()
    {
        character = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterHolder>();
    }

    public virtual void ReflectDamage(GameObject gameObjectToBlock)
    {
        if(oppositeDirection != null)
        {
            currentPosition = this.transform.localPosition;  

            Vector3 reflectDirection = oppositeDirection.localPosition - currentPosition + new Vector3(0, 0.7f, 0);

            gameObjectToBlock.GetComponent<Weapon>().isBouncedBack = true;
            gameObjectToBlock.GetComponent<Rigidbody>().velocity = reflectDirection.normalized * character.Force * character.Direction.magnitude;
            gameObjectToBlock.transform.up = -reflectDirection;
        }
    }
}
