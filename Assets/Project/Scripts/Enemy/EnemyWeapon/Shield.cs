using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour, ICanBlockDamage
{
    private CharacterHolder character;

    private void Start()
    {
        character = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterHolder>();
    }

    public virtual void BlockDamage(GameObject gameObjectToBlock, Vector3 oppositeDirection)
    {
        gameObjectToBlock.GetComponent<Weapon>().isBouncedBack = true;
        gameObjectToBlock.GetComponent<Rigidbody>().velocity = oppositeDirection.normalized * character.Force * character.Direction.magnitude;
        gameObjectToBlock.transform.Rotate(new Vector3(0,0,180), Space.World);
    }
}
