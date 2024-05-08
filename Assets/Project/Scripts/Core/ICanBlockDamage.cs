using UnityEngine;

public interface ICanBlockDamage
{
    public void BlockDamage(GameObject gameObjectToBlock,Vector3 oppositeDirection);
}
