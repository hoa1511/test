using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickUp : MonoBehaviour, IPickable
{
    [SerializeField] public WeaponType weaponType;
    private WeaponController weaponController;
    
    public void HandlePickItem()
    {
         weaponController.ApplyWeapon(weaponType);
         //Destroy(gameObject);
         gameObject.SetActive(false);
    }

    private void Start()
    {
        weaponController = WeaponController.Instance;
    }

    private void Update()
    {
        
    }
}
