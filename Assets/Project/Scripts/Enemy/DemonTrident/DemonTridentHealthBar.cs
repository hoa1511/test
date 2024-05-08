using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemonTridentHealthBar : MonoBehaviour
{
    [SerializeField] protected Image healthBar;
    [SerializeField] protected Transform enemy;
    private Quaternion rotation;

    public virtual void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        //Handle Update Health Bar
    }
    protected virtual void Start()
    {
        rotation = transform.rotation;
    }

    protected virtual void Awake()
    {
        
    }

    protected virtual void LateUpdate()
    {
        RotateHealthBar();
    }

    protected virtual void Update()
    {
        transform.position = new Vector3(enemy.transform.position.x, enemy.transform.position.y + 0.7f, enemy.transform.position.z);
    }

    protected virtual void RotateHealthBar()
    {
        transform.rotation = rotation;
    }
}
