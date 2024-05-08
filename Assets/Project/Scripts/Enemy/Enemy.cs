using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyType
{
    Boss,
    Archer,
    Fireman,
    Guard,
    Sword,
    Shark
}

public enum AnimationType
{
    IdleBattle,
    Die,
    Attack02,
    WalkFWD,
    RunFWD,
    GetHit
}

public class Enemy : MonoBehaviour, ICanTakeDamage, IDamageable, IPlaySound
{
    [SerializeField] protected PatrolPath patrolPath;
    [SerializeField] protected AudioClip enemyTakeDamageSFX;
    [SerializeField] protected float waypointDwellTime = 3f;
    [SerializeField] protected EnemyType enemyType;
    [SerializeField] protected EnemyHeadZone enemyHeadZone;
    [SerializeField] protected int enemyHealth;
    [SerializeField] protected float walkSpeed = 1;
    [SerializeField] protected float runSpeed = 2;
    [SerializeField] protected bool isFlyingType;
    [SerializeField] protected Animator enemyAnimator;
    [SerializeField] protected FieldOfView detectRangeFOV;
    [SerializeField] protected FieldOfView attackRangeFOV;
    [SerializeField] protected float delayTimeAttack = 0f;

    protected float waypointTolerance = 0.1f;
    protected float timeSinceArrivedAtWaypoint = 0;
    protected int currentWaypointIndex = 0;

    protected EnemyState enemyCurrentState;
    //protected NavMeshAgent navMeshAgent;
    protected AudioSource audioSource;
    protected Vector3 enemyPosition;
    protected GameObject player;
    private ICanTakeDamage client;
    protected DisplayNumberOfEnemy displayNumberOfEnemy;
    
    protected float detectRange = 1f;
    protected bool isDead;
    protected bool isStopped;
    protected bool isStun;

    private AnimationClip getHitClip, combatClip, dieClip;
    private int baseHealth;
    protected AnimatorUpdateMode animatorUpdateMode = AnimatorUpdateMode.UnscaledTime;

    protected virtual void Awake()
    {
        getHitClip = SetupAnimation(AnimationType.GetHit);
        combatClip = SetupAnimation(AnimationType.Attack02);
        dieClip = SetupAnimation(AnimationType.Die);

        enemyAnimator.updateMode = animatorUpdateMode;
        Debug.Log(enemyAnimator.updateMode);
    }

    protected virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        enemyCurrentState = new EnemyTauntState(gameObject);

        audioSource = GetComponent<AudioSource>();

        baseHealth = enemyHealth;

        //navMeshAgent = GetComponent<NavMeshAgent>();
 
        enemyPosition = transform.position;

        isDead = false;

        isStopped = false;

        if(baseHealth >= 3) {
            AnimationEvent getHitEvent = new AnimationEvent();
            getHitEvent.time = getHitClip.length;
            getHitEvent.functionName = "OnGetHitFinish";
            if(OnAddEvent(getHitClip, getHitEvent.functionName)) {
                getHitClip.AddEvent(getHitEvent);
            }
        }
        
        AnimationEvent combatEvent = new AnimationEvent();
        combatEvent.time = combatClip.length;
        Debug.Log(combatClip.length);
        combatEvent.functionName = "OnCombatFinish";
        if(OnAddEvent(combatClip, combatEvent.functionName)) {
            combatClip.AddEvent(combatEvent);
        }

        AnimationEvent damageEvent = new AnimationEvent();
        damageEvent.functionName = "KillPlayer";
        if(OnAddEvent(combatClip, damageEvent.functionName)) {
            combatClip.AddEvent(damageEvent);
        }
        
        AnimationEvent dieEvent = new AnimationEvent();
        dieEvent.time = dieClip.length;
        dieEvent.functionName = "OnDieFinish";
        if(OnAddEvent(dieClip, dieEvent.functionName)) {
            dieClip.AddEvent(dieEvent);
        }
    }

    private bool OnAddEvent(AnimationClip clip, string functionName) {
        foreach(AnimationEvent animationEvent in clip.events) {
            if(animationEvent.functionName.Equals(functionName)) {
                return false;
            }
        }
        return true;
    }

    private AnimationClip SetupAnimation(AnimationType type)
    {
        foreach(AnimationClip animationClip in enemyAnimator.runtimeAnimatorController.animationClips)
        {
            if (animationClip.name.Equals(type.ToString()))
            {
                return animationClip;
            }
        }
        return null;
    }

    private void OnGetHitFinish()
    {
        isStopped = false;
    }

    protected virtual void OnCombatFinish()
    {
        if (attackRangeFOV.canSeePlayer == false)
        {
            currentDelayTimeAttack = 0;
            isStopped = false;
        }
    }

    protected virtual void OnDieFinish()
    {

    }

    protected float currentDelayTimeAttack = 0f;
    protected virtual void Update()
    {
        if (Time.timeScale == 1 && animatorUpdateMode.Equals(AnimatorUpdateMode.UnscaledTime))
        {
            animatorUpdateMode = AnimatorUpdateMode.Normal;
            enemyAnimator.updateMode = animatorUpdateMode;
            EnemyIdle();
            return;
        }
        if (isDead == false)
        {
            if (detectRangeFOV.canSeePlayer == true)
            {
                if (attackRangeFOV.canSeePlayer == true)
                {
                    EnemyCombat();
                    return;
                }
                currentDelayTimeAttack += Time.deltaTime;
                if (currentDelayTimeAttack >= delayTimeAttack)
                {
                    if(isFlyingType) {
                        FlyTo(detectRangeFOV.targetPos.position);
                    } else {
                        if(baseHealth > 2) {
                            RunTo(detectRangeFOV.targetPos.position);
                        } else {
                            MoveTo(detectRangeFOV.targetPos.position);
                        }   
                    }
                }
            }
            else
            {
                PatrolBehaviour();
            }
        }
        UpdateTimers();
    }
    
#region Enemy States
    protected virtual void EnemyCombat()
    {
        StopEnemy();
        enemyCurrentState.Exit();
        enemyCurrentState = new EnemyCombatState(gameObject);
        enemyCurrentState.Enter();
    }


    protected virtual void EnemyIdle()
    {
        enemyCurrentState.Exit();
        enemyCurrentState = new EnemyIdleState(gameObject);
        enemyCurrentState.Enter();
    }

    protected virtual void EnemyRun()
    {
        enemyCurrentState.Exit();
        enemyCurrentState = new EnemyRunState(gameObject);
        enemyCurrentState.Enter();
    }

    protected virtual void EnemyDie()
    {
        displayNumberOfEnemy = GameObject.FindGameObjectWithTag("DisplayNumberOfEnemies").GetComponent<DisplayNumberOfEnemy>();
        displayNumberOfEnemy.UpdateNumberOfEnemyUI();

        GetComponent<Collider>().enabled = false;

        enemyCurrentState.Exit();
        enemyCurrentState = new EnemyDieState(gameObject);
        enemyCurrentState.Enter();

        enemyHeadZone.GetComponent<Collider>().enabled = false;

        if(isFlyingType)
        {
            GetComponent<Rigidbody>().isKinematic = false;
        }
        else
        {
            GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    protected virtual void EnemyWalkingState()
    {
        enemyCurrentState.Exit();
        enemyCurrentState = new EnemyWalkingState(gameObject);
        enemyCurrentState.Enter();
    }

    protected virtual void EnemyTauntState()
    {
        enemyCurrentState.Exit();
        enemyCurrentState = new EnemyTauntState(gameObject);
        enemyCurrentState.Enter();
    }

    protected virtual void EnemyGetHitState()
    {
        StopEnemy();
        enemyCurrentState.Exit();
        enemyCurrentState = new EnemyGetHitState(gameObject);
        enemyCurrentState.Enter();
    }

    protected virtual void EnemyRunState()
    {
        enemyCurrentState.Exit();
        enemyCurrentState = new EnemyRunState(gameObject);
        enemyCurrentState.Enter();
    }

    protected virtual void EnemySenseState()
    {
        StopEnemy();
        enemyCurrentState.Exit();
        enemyCurrentState = new EnemySenseState(gameObject);
        enemyCurrentState.Enter();
    }

#endregion

    public virtual void TakeDamage()
    {   
        if(isDead == false)
        {
            if(enemyHealth > 1)
            {
                EnemyGetHitState();
            }
            enemyHealth -= 1;
            if(enemyHealth <= 0)
            {
                EnemyDie();
                player.GetComponent<CharacterHolder>().expToUnlockSkin += 20;
                QuestManager.Instance.AchievementController.DoAchievementKill(enemyType);
                PlaySound(enemyTakeDamageSFX);
                isDead = true;
            }
        }
    }

    protected virtual void MoveTo(Vector3 destination)
    {
        if (isStopped == true) return;
        if(!isFlyingType) 
        {
            destination.y = transform.position.y;
        }
        
        transform.position = Vector3.MoveTowards(transform.position, destination, walkSpeed * Time.deltaTime);
        StartCoroutine(LookAt(new Vector3(destination.x, transform.position.y, destination.z)));
        EnemyWalkingState();
        //navMeshAgent.isStopped = false;
    }

    protected virtual void RunTo(Vector3 target)
    {
        if (isStopped == true) return;
        target.y = transform.position.y;
        transform.position = Vector3.MoveTowards(transform.position, target, runSpeed * Time.deltaTime);
        StartCoroutine(LookAt(new Vector3(target.x, transform.position.y, target.z)));
        EnemyRunState();
    }

    protected virtual void FlyTo(Vector3 target)
    {
        if (isStopped == true) return;
        transform.position = Vector3.MoveTowards(transform.position, target, runSpeed * Time.deltaTime);
        StartCoroutine(LookAt(new Vector3(target.x, transform.position.y, target.z)));
        EnemyRunState();
    }
    protected Vector3 GetCurrentWaypoint()
    {
        return patrolPath.GetWaypoint(currentWaypointIndex);
    }

    protected void StopEnemy()
    {
        isStopped = true;
        transform.Translate(Vector3.zero);
        EnemyIdle();
    }

    protected void CycleWaypoint()
    {
        currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
    }

    protected bool AtWaypoint()
    {
        float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
        return distanceToWaypoint <= waypointTolerance;
    }

    protected virtual void PatrolBehaviour()
    {
        Vector3 nextPosition = enemyPosition;

        if(isStopped == false)
        {
            if (patrolPath != null)
            {
                if (AtWaypoint())
                {
                    timeSinceArrivedAtWaypoint = 0;
                    CycleWaypoint();
                    EnemyIdle();
                }
                nextPosition = GetCurrentWaypoint();
            }
            
            if(timeSinceArrivedAtWaypoint > waypointDwellTime)
            {
                MoveTo(nextPosition);
            }
        } 
    }

    protected void UpdateTimers()
    {
        timeSinceArrivedAtWaypoint += Time.deltaTime;
    }

    private bool DetectPlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        return distanceToPlayer <= detectRange;
    }

    public void KillPlayer()
    {
        if(attackRangeFOV.canSeePlayer == false) return;
        client = GameObject.FindGameObjectWithTag("Player").GetComponent<ICanTakeDamage>();
        HandleDamage(client);
    }

    public void HandleDamage(ICanTakeDamage client)
    {
        client.TakeDamage();
    }

    protected IEnumerator LookAt(Vector3 destination)
    {
        Quaternion lookRotation = Quaternion.LookRotation(destination - transform.position);
        float time = 0;
        while(time < 1)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 0.02f);
            time += Time.deltaTime * 0.5f;
            yield return null;
        }
    }
    

    public virtual void PlaySound(AudioClip sound)
    {
        audioSource.PlayOneShot(sound);
    }

    public virtual void Stun()
    {
        isStun = true;
    }

    public virtual void EnemySpecialEffect()
    {

    }

    protected void OnBecameInvisible() {
        this.gameObject.SetActive(false);
    }

    protected void OnBecameVisible() {
        this.gameObject.SetActive(true);
    }
}
