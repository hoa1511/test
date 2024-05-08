using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBoss : WeaponBase
{
    private Vector3 direction;
    [HideInInspector] public GameObject player;

    [SerializeField] private Animator animator;
    public bool isFly = false;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "rightWall" || other.gameObject.tag == "leftWall" || other.gameObject.tag == "wall")
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
            WeaponShakeAnimation();
        }
        if(other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<CharacterHolder>().TakeDamage();
        }
    }

    private void WeaponShakeAnimation()
    {
        animator.SetBool("WeaponHitWall", true);
    }

    protected override void Update()
    {
        base.Update();
        direction = player.transform.position - transform.position;
    }

    public void Attack(Vector3 direction)
    {
        GetComponent<Rigidbody>().AddForce(direction.normalized * 10, ForceMode.Impulse);
        isFly = true;
    }

    public void OnFlashToWeapon()
    {
        isFly = false;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ;
    }
}
