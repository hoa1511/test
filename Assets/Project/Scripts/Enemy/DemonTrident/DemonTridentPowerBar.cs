using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonTridentPowerBar : DemonTridentHealthBar
{
    protected override void Update()
    {
        transform.position = new Vector3(enemy.transform.position.x, enemy.transform.position.y + 0.5f, enemy.transform.position.z);
    }
}
