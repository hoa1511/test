using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public enum BossPhase
{
    PHASE1,
    PHASE2
}

public enum BossAttackSkill { 
    BASE_ATTACK,
    ATTACK_CALL_TRIDENT,
    ULTI_SKILL
}

public class BossBehaviour : MonoBehaviour, ICanTakeDamage, IPlaySound
{
    [SerializeField] protected EnemyType enemyType;
    [SerializeField] protected AudioClip screamSound;
    [SerializeField] protected Vector3[]  posCubeMoving;
    [SerializeField] public Vector3[]  posCubeUltiMoving;
    [SerializeField] protected GameObject cubeLookAt;
    [SerializeField] protected GameObject cubeSpawnUlti;
    [SerializeField] protected Character character;
    [SerializeField] protected GameObject modelWeapon;
    [SerializeField] protected WeaponBoss weaponBoss;
    [SerializeField] protected GameObject flashParticleSystem;
    [SerializeField] protected GameObject UltiParticleSystem;
    [SerializeField] protected GameObject bossUltiObject;

    [SerializeField] Image heathBarFillImage;
    [SerializeField] Image powerBarFillImage;

    protected float currentHealthValue;
    protected float maxHealthValue;
    
    public int currentPowerValue;
    public int maxPowerValue;

    [SerializeField] protected float timeToAttack, timeMovingCubeObject, timeMovingCubeUlti;
    [HideInInspector] public float currentTimeToAttack = 0f;
    [SerializeField] protected int minCount,maxCount;
    [SerializeField] protected float numberOfAttackToUseUltimate;
    
    
    protected int countToAttackPlayer = 0;
    protected int currentCountToAttackPlayer = 0;
    protected float countToUseUltimate = 0;
    protected bool isDead = false;
    protected bool hasPlayUlti = false;

    protected DisplayNumberOfEnemy displayNumberOfEnemy;

    protected BossAttackSkill bossAttackSkill = BossAttackSkill.BASE_ATTACK;
    protected BossPhase bossPhase = BossPhase.PHASE1;
    protected Vector3 direction;
    protected AudioSource  audioSource;

    private Sequence sequenceCube;
    private Sequence sequenceSkill;

    public virtual void Awake()
    {
        countToAttackPlayer = Random.Range(minCount, maxCount);
        Shuffle(posCubeMoving);

        cubeLookAt.transform.position = posCubeMoving[0];
    }

    public virtual void Start()
    {
        sequenceCube = DOTween.Sequence();
        sequenceSkill = DOTween.Sequence();

        audioSource = GetComponent<AudioSource>();

        maxHealthValue = 3;
        currentHealthValue = maxHealthValue;
        heathBarFillImage.fillAmount = currentHealthValue/maxHealthValue;
        
        maxPowerValue = 2;
        currentPowerValue = 0;

        MoveBossLookAtTarget();
        MoveBossSpawnUlti();
    }

    public virtual void Update()
    {
        if(isDead == false)
        {
            if(countToUseUltimate == numberOfAttackToUseUltimate)
            {
                bossAttackSkill = BossAttackSkill.ULTI_SKILL;
            }

            if(currentCountToAttackPlayer >= countToAttackPlayer)
            {
                transform.LookAt(weaponBoss.player.transform);
                direction = weaponBoss.player.transform.position - transform.position;

                if(weaponBoss.isFly == false)
                {
                    weaponBoss.transform.eulerAngles = new Vector3(0, 0, CalculateAngle(direction, weaponBoss.player.transform));
                }
            }

            else 
            {
                transform.LookAt(cubeLookAt.transform);

                direction = cubeLookAt.transform.position - transform.position;

                if(weaponBoss.isFly == false)
                {
                    weaponBoss.transform.eulerAngles = new Vector3(0, 0, CalculateAngle(direction, cubeLookAt.transform));;
                }
            }

            currentTimeToAttack += Time.deltaTime;

            if(currentTimeToAttack >= timeToAttack)
            {
                if(bossAttackSkill == BossAttackSkill.BASE_ATTACK)
                {
                    BaseAttack();
                }
                if(bossAttackSkill == BossAttackSkill.ULTI_SKILL)
                {
                    if(hasPlayUlti == false)
                    {
                        StartCoroutine(UltiAttack());
                        hasPlayUlti = true;
                    }
                }
            }
        }
    }

    public virtual void Shuffle(Vector3[] array)
    {
        for (int t = 0; t < array.Length; t++)
        {
            Vector3 tmp = array[t];
            int r = Random.Range(t, array.Length);
            array[t] = array[r];
            array[r] = tmp;
        }
    } 

    public virtual void SpawnParticleSystem()
    {
        GameObject flashparticle = Instantiate(flashParticleSystem, transform.position, Quaternion.identity);
        flashparticle.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        Destroy(flashparticle, 2);
    }

    public virtual void SpawnUltiParticleSystem()
    {
        GameObject flashparticle = Instantiate(UltiParticleSystem, transform.position, Quaternion.identity);
        Destroy(flashparticle, 5);
    }

    public virtual float CalculateAngle(Vector3 direction, Transform objectLookAt)
    {
        float angle = Vector2.Angle(direction, -Vector2.up);
        if(weaponBoss.transform.position.x > objectLookAt.position.x)
        {
            angle = -angle;
        }
        return angle;
    }

    public virtual void TeleportWeapon()
    {
        weaponBoss.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
        transform.position = weaponBoss.transform.position;
        weaponBoss.gameObject.SetActive(false);
        SpawnParticleSystem();
        weaponBoss.OnFlashToWeapon();
    }

    public virtual void BaseAttack()
    {
        if(character.animator.GetBool("isFlash") == false)
        {
            modelWeapon.SetActive(true);

            character.animator.SetBool("isFlash", true);
        }
    }

    public virtual IEnumerator UltiAttack()
    {
        SpawnUltiParticleSystem();

        for(int i = 0; i < 10; i++)
        {
            GameObject ultiTrident = Instantiate(bossUltiObject, cubeSpawnUlti.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(1f);
        countToUseUltimate = 0;
        bossAttackSkill = BossAttackSkill.BASE_ATTACK;
        hasPlayUlti = false;

    }

    public virtual void FixedUpdate()
    {
        if(character.canThrow)
        {
            character.canThrow = false;

            modelWeapon.SetActive(false);

            weaponBoss.gameObject.SetActive(true);

            weaponBoss.Attack(direction);

            if(currentCountToAttackPlayer >= countToAttackPlayer)
            {
                currentCountToAttackPlayer = 0;
                countToAttackPlayer = Random.Range(minCount, maxCount);
            }

            else
            {
                currentCountToAttackPlayer += 1;
            }
            countToUseUltimate += 1;
        }
        powerBarFillImage.fillAmount = countToUseUltimate/numberOfAttackToUseUltimate;
    }

    public virtual void TakeDamage()
    {
        displayNumberOfEnemy = GameObject.FindGameObjectWithTag("DisplayNumberOfEnemies").GetComponent<DisplayNumberOfEnemy>();
        currentHealthValue -= 1;
        heathBarFillImage.fillAmount = 1 - (maxHealthValue - currentHealthValue)/maxHealthValue;
        if(currentHealthValue == 0)
        {
            isDead = true;
            PlaySound(screamSound);
            character.animator.SetBool("isDead", true);
            GetComponent<Rigidbody>().useGravity = true;
            displayNumberOfEnemy.UpdateNumberOfEnemyUI();
            QuestManager.Instance.DailyQuestController.DoDailyQuest(QuestType.Kill);
            QuestManager.Instance.AchievementController.DoAchievementKill(enemyType);
        }
    }

    public virtual void MoveBossLookAtTarget()
    {
        sequenceCube.Append(cubeLookAt.transform.DOMove(posCubeMoving[0], timeMovingCubeObject))
        .Append(cubeLookAt.transform.DOMove(posCubeMoving[1], timeMovingCubeObject))
        .Append(cubeLookAt.transform.DOMove(posCubeMoving[2], timeMovingCubeObject))
        .Append(cubeLookAt.transform.DOMove(posCubeMoving[3], timeMovingCubeObject))
        .Append(cubeLookAt.transform.DOMove(posCubeMoving[0], timeMovingCubeObject))
        .SetLoops(-1);
    }

    public virtual void MoveBossSpawnUlti()
    {
        sequenceSkill.Append(cubeSpawnUlti.transform.DOMove(posCubeUltiMoving[0], timeMovingCubeUlti))
        .Append(cubeSpawnUlti.transform.DOMove(posCubeUltiMoving[1], timeMovingCubeUlti))
        .Append(cubeSpawnUlti.transform.DOMove(posCubeUltiMoving[0], timeMovingCubeUlti))
        .SetLoops(-1);
    }

    public virtual void PlaySound(AudioClip sound)
    {
        audioSource.PlayOneShot(sound);
    }
}
