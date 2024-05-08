using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerBar : MonoBehaviour
{
    [SerializeField] protected Image healthBar;
    Transform player;
    private Quaternion rotation;

    protected virtual void Start()
    {
        rotation = transform.rotation;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected virtual void LateUpdate()
    {
        RotateHealthBar();
    }

    protected virtual void Update()
    {
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 0.7f, player.transform.position.z);
    }

    protected virtual void RotateHealthBar()
    {
        transform.rotation = rotation;
    }
}
