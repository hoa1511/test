using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWereWolf : Enemy
{
    [Header("Trails")]
    [SerializeField] private GameObject leftTrail;
    [SerializeField] private GameObject rightTrail;
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void OnCombatFinish()
    {
        base.OnCombatFinish();
    }
}
