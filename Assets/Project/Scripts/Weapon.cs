using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DynamicMeshCutter;
public class Weapon : WeaponBase
{
    [Header("Sound FX")]
        [SerializeField] private AudioClip hitShieldSFX;
        [SerializeField] private AudioClip tridentHitWallSFX;

    [Header("Character Holder")]
        [SerializeField] private CharacterHolder player;

    [Header("Trails")]
        [SerializeField] private Transform trailHolder;
        [SerializeField] private GameObject currentTrailGameObject;
        [SerializeField] private TrailRenderer trail;

    [Header("Cut Objects")]
        [SerializeField] private CutterBehaviour cutterBehaviour;

    [Header("Buff Speed")]
        [SerializeField] private float multiplier = 1;

    [HideInInspector] public bool isBouncedBack = false; 

    private PlayerWeapon playerWeapon;
    private Animator animator;
    private Camera mainCamera;

    public float Multiplier {get => multiplier; set => multiplier = value;}

    private void Awake()
    {
        InstantiateWeaponSkin();
    }

    protected override void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animator = transform.GetChild(0).GetComponent<Animator>();
        mainCamera = Camera.main;
    }

    private void InstantiateWeaponSkin()
    {
        playerWeapon = WeaponSkinLoader.Instance.PlayerWeapon;
        transform.GetChild(0).GetChild(1).GetComponent<Renderer>().material = playerWeapon.weaponMaterial;

        LoadSkinVFX(playerWeapon);
    }

    protected override void Update() 
    {
        // Check if the player reaches the edge of the screen
        Vector3 viewportPos = mainCamera.WorldToViewportPoint(transform.position);
        if (viewportPos.x <= 0f || viewportPos.x >= 1f)
        {
            trail.widthMultiplier = 0f;
            TeleportTrident();
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<ICutable>(out ICutable cutableClient)) 
        {
            cutableClient.CutObject(cutterBehaviour);
        }
        
        if(other.gameObject.tag == "wall")
        {
            PlaySound(tridentHitWallSFX);
            GetComponent<Collider>().isTrigger = false;
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            GetComponent<Rigidbody>().velocity = Vector3.zero;      
            WeaponShakeAnimation();
            StartCoroutine(MovePlayerToTrident());
        }

        if(other.gameObject.TryGetComponent<ICanBlockDamage>(out ICanBlockDamage client))
        {
            PlaySound(hitShieldSFX);
            client.BlockDamage(this.gameObject, -player.Direction);
        }

        if(other.gameObject.TryGetComponent<IReflectDamage>(out IReflectDamage reflectClient))
        {
            PlaySound(hitShieldSFX);
            reflectClient.ReflectDamage(this.gameObject);
        }

        if(other.gameObject.TryGetComponent<IPickable>(out IPickable pickupClient))
        {
            pickupClient.HandlePickItem();
        }
        
        if(isBouncedBack && other.gameObject.CompareTag("Player"))
        {
            isBouncedBack = false;
            base.OnTriggerEnter(other);            
        }

        if(!other.gameObject.CompareTag("Player"))
        {
            base.OnTriggerEnter(other);            
        }
    }

    public override void HandleDamage(ICanTakeDamage client)
    {
        //AOE Range Is For Skin Which Always Has AOE Effect 
        float AOERange = CharacterStatController.Instance.GetCurrentSkinStat(PassiveStats.AreaOfEffect);
        //AOE Chance Is For Skin Which Has AOE Chance
        float AOEChance = CharacterStatController.Instance.GetCurrentSkinStat(PassiveStats.AreaOfEffectPercent);
        base.HandleDamage(client);
        
        float randomNumber = Random.Range(0, 100);
        if(randomNumber < AOEChance)
        {
            DealAOEDamage(100);
        }
        DealAOEDamage(AOERange);
    }

    private void WeaponShakeAnimation()
    {
        animator.SetBool("WeaponHitWall", true);
    }

    public void DisableWeapon()
    {
        gameObject.SetActive(false);
    }

    private void DealAOEDamage(float range)
    {
        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, range);
        if(hitColliders.Length > 0)
        {
            foreach(var hitCollider in hitColliders)
            {
                if(hitCollider.TryGetComponent<ICanTakeDamage>(out ICanTakeDamage takeDamageClient) && !hitCollider.CompareTag("Player"))
                {
                    takeDamageClient.TakeDamage();
                }
            }
        }
    }

    private void LoadSkinVFX(PlayerWeapon weapon)
    {
        if(weapon.weaponTrailEffect != null)
        {
            foreach(Transform child in trailHolder)
            {
                Debug.Log(child.name);
                if(child.name == weapon.weaponTrailEffect.name)
                {
                    currentTrailGameObject = child.gameObject;
                    currentTrailGameObject.SetActive(true);
                }
                else
                {
                    child.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            currentTrailGameObject = trailHolder.transform.GetChild(0).gameObject;
            currentTrailGameObject.SetActive(true);
            
        }
    }

    private IEnumerator MovePlayerToTrident()
    {
        yield return new WaitForSeconds(0.3f);
        player.MoveToTrident();
    }

    private void TeleportTrident()
    {
        Vector3 collisionPos = transform.position;
        Quaternion rotation = transform.rotation;

        float idx = 0; 

        if(collisionPos.x < 0)
        {
            idx = 0.3f;
        }
        else if(collisionPos.x > 0)
        {
            idx = -0.3f;
        }

        transform.position = new Vector3(-collisionPos.x - idx, collisionPos.y, collisionPos.z);
        transform.rotation = rotation;
        
        StartCoroutine(turnOnTrail());
    }

    private IEnumerator turnOnTrail()
    {
        yield return new WaitForSeconds(0.2f);
        trail.widthMultiplier = 1.0f;
    }
}
