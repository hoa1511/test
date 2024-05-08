using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilMerge : WeaponBase
{
    [SerializeField] private float speed = 10;
    GameObject characterHolder;

    private Vector3 playerPos;
    private Vector3 directionVector;
    private Vector3 bulletVelocity;
    public Vector3 spawnPos;

    protected override void Start()
    {
        characterHolder = GameObject.FindGameObjectWithTag("Player");
        playerPos = characterHolder.transform.position;
        directionVector = playerPos - spawnPos;
        directionVector.Normalize();

        base.Start();
    }

    protected override void Update()
    {
        MoveBullet();
        base.Update();
    }

    private void FixedUpdate()
    {
        

        bulletVelocity = directionVector * speed;
    }

    private void MoveBullet()
    {
        float deltaTime = Time.deltaTime;
        transform.position += bulletVelocity * deltaTime;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "wall")
        {
            Destroy(this.gameObject);
        }

        if(other.CompareTag("Player"))
        {
            base.OnTriggerEnter(other);
            Destroy(this.gameObject);
        }
    }
}
