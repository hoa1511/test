using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterHolder : MonoBehaviour, ICanTakeDamage, IPlaySound
{
    [SerializeField] Image powerBarFillImage;
    [SerializeField] public Transform middle, weapon, cubeLookAt, statusTextHolder;
    [SerializeField] private LineRenderer playerLineRenderer;
    [SerializeField] private AudioClip throwWeaponSFX;
    [SerializeField] private AudioClip playerDeadSFX;
    [SerializeField] private Transform virtualCube; 
    [SerializeField] public float maxForce;
    [SerializeField] private GameObject teleportFx;
    [SerializeField] private Transform parent;
    [SerializeField] private GameObject currentSkin;
    [SerializeField] private float startYPos  = 0;
    private Character character;
    private LineRenderer lineRenderer;
    private GameObject modelWeapon;
    
    
    [HideInInspector] public bool isDead = false;
    [HideInInspector] public bool isReadyToLauch = false;

    private bool hasSpawnLooseUI = false;
    private bool hasSpawnParticle = false;
    private bool hasSpawnSkinParticle = false;
    public bool isWaiting = false;
    public bool firstPhase = true;
    private bool hasThrow = false;
    private bool isTouchWall = false;

    public float distanceFinger;
    private Vector3 direction;
    public Weapon tridenWeapon;

    Animator animator;
    AudioSource audioSource;
    GameObject instantiateSkin;

    public float force;
    private float currentPowerValue;
    private float maxPowerValue = 1;
    private float currentTime = 0;
    private float currentActiveShieldTime = 0;
    private float activeImortalShieldTime;
    private Rigidbody rbTrident;

    public int expToUnlockSkin;

    public float Force { get=>force; }
    public Vector3 Direction {get => direction;}

    private void Awake()
    {
        InstantiateSkin();  
    }

    private void InstantiateSkin()
    {
        this.gameObject.transform.localPosition = new Vector3(0, 0, 0);
        transform.parent.position = new Vector3(0, startYPos, 0);
        
        Vector3 currentPos = new Vector3(0, -3.85f, 0);
        Vector3 currentScale = new Vector3(7, 7, 7);
 
        Destroy(currentSkin.gameObject);

        instantiateSkin = Instantiate(SkinLoader.Instance.LastSavedSkin, parent);
        instantiateSkin.transform.localScale = currentScale;
        instantiateSkin.transform.localPosition = currentPos;
        instantiateSkin.GetComponent<Character>().player = parent.GetComponent<CharacterHolder>();

        character = instantiateSkin.GetComponent<Character>();
        lineRenderer = instantiateSkin.transform.GetChild(4).GetComponent<LineRenderer>();
        middle = GameObject.FindGameObjectWithTag("Middle").transform;
    }

    void Start()
    {
        playerLineRenderer.enabled = false;
        isDead = false;
        hasSpawnLooseUI = false;

        this.GetComponent<Rigidbody>().isKinematic = true;
        rbTrident = weapon.GetComponent<Rigidbody>();

        activeImortalShieldTime = CharacterStatController.Instance.GetCurrentSkinStat(PassiveStats.ImmortalShield);
        float bonusTridentSpeed = 8 * CharacterStatController.Instance.GetCurrentSkinStat(PassiveStats.PercentIncreaseTridentSpeed)/100;
        force = 8 + bonusTridentSpeed;

        currentPowerValue = maxPowerValue;
        powerBarFillImage.fillAmount = currentPowerValue/maxPowerValue;

        audioSource = GetComponent<AudioSource>();
        animator = weapon.GetChild(0).GetComponent<Animator>();
        
        tridenWeapon = weapon.GetComponent<Weapon>();

        lineRenderer.positionCount = 2;
        
        weapon.gameObject.SetActive(false);
        weapon.transform.position = new Vector3(middle.position.x, middle.position.y, 0);

        instantiateSkin.transform.GetChild(0).GetChild(5).GetChild(1).GetChild(1).GetComponent<Renderer>().material = WeaponSkinLoader.Instance.PlayerWeapon.weaponMaterial;
        modelWeapon = instantiateSkin.transform.GetChild(0).GetChild(5).GetChild(1).GetChild(1).gameObject;
    }

    private IEnumerator EnableControl()
    {
        yield return new WaitForSeconds(0.5f);
        firstPhase = false;
    }

    void Update()
    {
        ImmotalShield(activeImortalShieldTime,5);

        if(isWaiting == true)
        {
            Time.timeScale = 0;
        }

        if(isDead == false && weapon != null && PlayerPrefs.GetInt("hasPause") == 0 && isWaiting == false && GameManager.Instance.hasWon == false)
        {
            transform.LookAt(cubeLookAt);
            if (Input.GetMouseButtonDown(0))
            {
                virtualCube.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
                MoveToTrident();
            }

            if (Input.GetMouseButton(0) && hasThrow == false)
            {
                playerLineRenderer.enabled = true;
                GameManager.Instance.StartSlowMotion();
                distanceFinger = Vector3.Distance(lineRenderer.GetPosition(1), lineRenderer.GetPosition(0));
                currentPowerValue -= Time.deltaTime;
                powerBarFillImage.fillAmount = currentPowerValue/maxPowerValue;
                character.animator.SetBool("isPose", true);
                // modelWeapon.SetActive(true);

                ActiveParticleSystem();   
                SpawnParticleSystem();

                weapon.transform.eulerAngles = new Vector3(0, 0, CalculateAngle());
                weapon.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

                Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
                cubeLookAt.position = new Vector3(middle.position.x, middle.position.y, -1) +  direction.normalized;

                lineRenderer.SetPosition(0, new Vector3(virtualCube.position.x, virtualCube.position.y, 0));
                lineRenderer.SetPosition(1, Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z)));

                playerLineRenderer.SetPosition(0, new Vector3(middle.position.x, middle.position.y, 0));
                playerLineRenderer.SetPosition(1, new Vector3(middle.position.x, middle.position.y, 0) + direction);

                this.GetComponent<Rigidbody>().isKinematic = false;   
            }
           
            if (Input.GetMouseButtonUp(0) && hasThrow == false || currentPowerValue <= 0 && hasThrow == false && isWaiting == false)
            {
                distanceFinger = 0;
                this.GetComponent<Rigidbody>().isKinematic = true;
                

                hasSpawnParticle = false;
                hasSpawnSkinParticle = false;

                transform.GetChild(1).gameObject.SetActive(false);
                transform.GetChild(4).gameObject.GetComponent<Character>();

                GameManager.Instance.StopSlowMotion();

                if (isReadyToLauch == true)
                {
                    weapon.gameObject.SetActive(true);
                    modelWeapon.SetActive(false);

                    character.canThrow = true;
                    character.animator.SetBool("isPose", false);
                    character.animator.SetBool("isThrow", true);

                    hasThrow = true; 
                    isReadyToLauch = false;
                    PlaySound(throwWeaponSFX);
                } 
                
                else
                {
                    character.animator.SetBool("isPose", true);
                    character.animator.SetBool("isFlash", false);
                    character.animator.SetBool("isThrow", false);
                }

                playerLineRenderer.enabled = false;
                lineRenderer.enabled = false; 
            }
        }
    }

    private float CalculateAngle()
    {
        direction = lineRenderer.GetPosition(1) - lineRenderer.GetPosition(0);

        if(Vector3.Distance(lineRenderer.GetPosition(1), lineRenderer.GetPosition(0)) >= maxForce)
        {
            direction = direction.normalized * maxForce;
        }
        float angle = Vector2.Angle(direction, -Vector2.up);
        if (lineRenderer.GetPosition(1).x < lineRenderer.GetPosition(0).x)
        {
            angle = -angle;
        }

        return angle;
    }

    private void FixedUpdate()
    {
        if (character.canThrow)
        {
            float multiplier = tridenWeapon.Multiplier;
            weapon.transform.position = new Vector3(middle.position.x, middle.position.y, 0);
            rbTrident.AddForce(direction * force * multiplier, ForceMode.Impulse);
            character.canThrow = false;
        }

        var dir = rbTrident.velocity;
        if (dir != Vector3.zero)
            weapon.transform.rotation = Quaternion.LookRotation(dir);
    }

    public void TakeDamage()
    {
        if(isDead == false)
        {
            float evadePercent = CharacterStatController.Instance.GetCurrentSkinStat(PassiveStats.EvadePercent);
            int randomNumber = Random.Range(0, 100);

            if(randomNumber < evadePercent)
            {
                Debug.Log("Miss Skill");
                StatusTextController.Instance.InstantiateStatusText("Miss", StatusTextController.Instance.missSprite, this.transform.position, Color.white, 0.3f);
            }
            else
            {
                DeathEvent();
            }
        }
    }
#region "Death Event"
    private void DeathEvent()
    {
        Time.timeScale = 1;
        isDead = true;

        character.animator.SetBool("IsDead", true);
        transform.GetChild(0).gameObject.SetActive(false);
        GetComponent<Collider>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = false;
        PlaySound(playerDeadSFX);

        GameManager.Instance.hasLoose = true;
        StartCoroutine(GameManager.Instance.DisPlayLooseUI());
    }

#endregion

#region "Imortal SKill"
    private void ImmotalShield(float activeTime, float waitTime)
    {
        if(isDead == false)
        {
            if(currentTime > waitTime)
            {   
                ActiveImortalShield(activeTime);
            }
            else
            {
                GetComponent<Collider>().enabled = true;
                currentTime += Time.deltaTime;
            }
        }
    }

    private void ActiveImortalShield(float activeTime)
    {
        if(currentActiveShieldTime < activeTime)
        {
            GetComponent<Collider>().enabled = false;
            currentActiveShieldTime += Time.deltaTime;
        }
        else
        {
            currentActiveShieldTime = 0;
            currentTime = 0;
        }
    }

#endregion

    public void WeaponDisappearBehavior()
    {
        weapon.gameObject.SetActive(false);
    }

    private void ActiveParticleSystem()
    {
        if(hasSpawnParticle == false)
        {
            transform.GetChild(1).gameObject.SetActive(true);
            hasSpawnParticle = true;
        }
    }

    private void SpawnParticleSystem()
    {
        if(transform.GetChild(4).gameObject.GetComponent<Character>().characterParticleSystem != null)
        {
            if(hasSpawnSkinParticle == false)
            {
                ParticleSystem skinParrticleSystem = Instantiate(transform.GetChild(4).gameObject.GetComponent<Character>().characterParticleSystem, transform.position, Quaternion.identity);
                Destroy(skinParrticleSystem.gameObject, 1);
                hasSpawnSkinParticle = true;
            }
        }
    }

    public void SetIsReadyToLaunch(bool isReadyToLauch)
    {
        this.isReadyToLauch = isReadyToLauch;
    }

    public void PlaySound(AudioClip sound)
    {
        if(isDead == false)
        {
            audioSource.PlayOneShot(sound);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "wall")
        {
            GetComponent<Rigidbody>().isKinematic = true;
        }

        if(other.gameObject.tag == "UpCollider")
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
        }

        if(other.gameObject.tag == "DownCollider")
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.1f, transform.position.z);
        }

        if(other.gameObject.tag == "RightCollider")
        {
            transform.position = new Vector3(transform.position.x - 0.1f, transform.position.y, transform.position.z);
        }

        if(other.gameObject.tag == "LeftCollider")
        {
            transform.position = new Vector3(transform.position.x + 0.1f, transform.position.y - 0.1f, transform.position.z);
        }
    }

    public void MoveToTrident()
    {
        //playerLineRenderer.enabled = true;
        
        if(isReadyToLauch == false)
        {
            currentPowerValue = maxPowerValue;
            hasThrow = false;

            transform.position = new Vector3(weapon.position.x, weapon.position.y, transform.position.z);

            GameObject teleportFX =  Instantiate(teleportFx, transform.position, Quaternion.identity);
            Destroy(teleportFX.gameObject, 1f);

            weapon.GetComponent<Weapon>().isBouncedBack = false;
            tridenWeapon.DisableWeapon();

            weapon.transform.position = new Vector3(weapon.position.x, weapon.position.y, transform.position.z);
            weapon.GetComponent<Rigidbody>().velocity = Vector3.zero;
            weapon.GetComponent<Collider>().isTrigger = true;
            
            character.animator.SetBool("isFlash", true);
            character.animator.SetBool("isThrow", false);

            virtualCube.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
        }
        modelWeapon.SetActive(true);
    }

}
