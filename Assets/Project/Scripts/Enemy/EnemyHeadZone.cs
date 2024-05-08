using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHeadZone : MonoBehaviour, ICanTakeDamage
{
    public void TakeDamage()
    {
        Enemy enemy = this.GetComponentInParent<Enemy>();
        enemy.TakeDamage();
        StatusTextController.Instance.InstantiateStatusText("Head Shot", StatusTextController.Instance.headShotSprite, this.transform.parent.transform.position, Color.red, 0.1f);
    }
}
