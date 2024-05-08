using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Character : MonoBehaviour
{
    [HideInInspector]
    public Animator animator;
    [HideInInspector]
    public bool canThrow = false;
    public bool canMoveToWeapon = true;
    [SerializeField] private BossBehaviour boss;
    [SerializeField] public CharacterHolder player;

    [SerializeField] public ParticleSystem characterParticleSystem;
    
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void TeleportWeapon()
    {
        boss.TeleportWeapon();
    }
    public void FlashToPose()
    {
        animator.SetBool("isFlash", false);
        animator.SetBool("isPose", true);
    }
    public void PoseToAttack()
    {
        animator.SetBool("isThrow", true);
        canThrow = true;
    }
    public void Throw()
    {
        animator.SetBool("isThrow", false);
        animator.SetBool("isPose", false);
        player.isReadyToLauch = false;
    }
    public void ThrowBoss()
    {
        animator.SetBool("isThrow", false);
        animator.SetBool("isFlash", false);
        // animator.SetBool("isAttack", false);
        boss.currentTimeToAttack = 0;
    }
    public void IsReadyToLauch()
    {
        player.isReadyToLauch = true;
    }
}