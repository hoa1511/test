using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public static WeaponController Instance;
    private GameObject currentWeapon = null;
    private CharacterHolder player;
    public GameObject weapon;

    private void Awake() 
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    [System.Serializable]
    public struct WeaponMapping
    {
        public WeaponType weaponType;
        public GameObject weaponMesh;
        public GameObject weaponItemMesh;
    }

    [SerializeField] public WeaponMapping[] weaponMappings;



    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterHolder>();
    }
    public WeaponMapping GetWeapon(WeaponType weaponType)
    {
        foreach(WeaponMapping weaponMapping in weaponMappings)
        {
            if(weaponMapping.weaponType == weaponType)
            {
                return weaponMapping;
            }
        }
        return weaponMappings[0];
    }

    public void ApplyWeapon(WeaponType weaponType)
    {
        WeaponMapping weaponMapping  = GetWeapon(weaponType);

        if(weapon != null)
        {
            currentWeapon = weaponMapping.weaponMesh;

            weapon = Instantiate(currentWeapon); 
            
            weapon.transform.position = player.weapon.transform.position;
            weapon.transform.rotation = player.weapon.transform.rotation;
            
            //weapon.gameObject.SetActive(false);
            player.weapon = weapon.transform;

            //weapon.transform.position = 
            player.tridenWeapon = weapon.GetComponent<Weapon>();
        }
    }
}
