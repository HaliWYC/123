using ShanHai_IsolatedCity.Map;
using System.Collections;
using UnityEngine;
using ShanHai_IsolatedCity.Skill;

[RequireComponent(typeof(CharacterInformation))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Skills))]
[RequireComponent(typeof(LootSpawner))]
public class EnemyController : MonoBehaviour,IEndGameObserver
{
    [Header("基本信息")]
    public int NPCID;
    public string initialScene;
    [HideInInspector]
    public GameObject attackTarget;
    private Collider2D coll;//Collider on the enemy
    private float speed;//Current Speed
    protected Animator anim;
    protected CharacterInformation enemyInformation;
    protected NPCDetails npcDetails;
    public Skills skills;

    [Header("状态")]
    private bool canAction;
    public bool isPeace;
    public bool isChase;
    private bool isMoving;
    private bool isDead;
    private bool playerDead;

    [Header("战斗")]
    [HideInInspector]
    public bool canAttack;//Can the enemy do the next attack action(Including the skill)
    [HideInInspector]
    protected float attackCoolingTime;//Only for base attack Action exclude SkillSpellCooling
    protected float timeAfterSkillSpell;
    public float dizzytime;
    public float sightRadius;

    [Header("位置")]
    private float direction;
    private Vector3 originalPos;
    [HideInInspector]
    public Vector3 attackPos;
    private bool isObstacle;
    public Transform playerTransform;

    [Header("巡逻范围")]
    public float PatrolRange;
    private Vector3 wayPoint;//The next automatic patrol position
    private float remainLookAtTime;
    public float lookAtTime;//The time between the any action has changed

    private void Awake()
    {
        anim = GetComponent<Animator>();
        coll = GetComponent<BoxCollider2D>();
        remainLookAtTime = lookAtTime;
        enemyInformation = GetComponent<CharacterInformation>();
        direction = transform.GetComponent<Transform>().localScale.x;
        canAttack = true;
        dizzytime = 0;
        canAction = true;

    }

    private void OnEnable()
    {
        //GameManager.Instance.AddObserver(this);
        EventHandler.AfterSceneLoadedEvent += OnAfterSceneLoadedEvent;
        EventHandler.UpdateGameStateEvent += OnUpdateGameStateEvent;
    }

    

    private void OnDisable()
    {
        if (!GameManager.isInitialized) return;
        GameManager.Instance.RemoveObserver(this);
        EventHandler.AfterSceneLoadedEvent -= OnAfterSceneLoadedEvent;
        EventHandler.UpdateGameStateEvent -= OnUpdateGameStateEvent;

        if (TaskManager.isInitialized && isDead)
            EventHandler.CallUpdateTaskProgressEvent(npcDetails.NPCName, 1);
    }

    private void OnAfterSceneLoadedEvent()
    {
        CheckVisible();
    }

    private void OnUpdateGameStateEvent(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.GamePlay:
                canAction = true;
                break;
            case GameState.Pause:
                canAction = false;
                break;
        }
    }

    protected virtual void Start()
    {
        npcDetails = NPCManager.Instance.GetNPCDetail(NPCID);
        speed = enemyInformation.Speed;
        originalPos = transform.position;
        if (isPeace)
        {
            npcDetails.enemyState = EnemyState.和平;
        }
        else
        {
            npcDetails.enemyState = EnemyState.巡逻;
            GenerateWayPoint();
            
        }

        attackCoolingTime = 0;
        timeAfterSkillSpell = 0;
        //FIXME:在加载场景的时候使用
        GameManager.Instance.AddObserver(this);
    }

    protected virtual void Update()
    {
        isDead = enemyInformation.CurrentHealth == 0;

        if (!playerDead  && dizzytime<=0 && canAction)
        {
            anim.SetBool("isDizzy", false);
            if (attackCoolingTime<=0 || timeAfterSkillSpell<=0)
                SwitchStates();
            SwitchAnimation();
            
            if(attackCoolingTime>0)
            attackCoolingTime -= Time.deltaTime;
            if (timeAfterSkillSpell > 0)
            timeAfterSkillSpell -= Time.deltaTime;

        }
        else if (playerDead || !canAction)
        {
            anim.SetBool("isMoving", false);
            anim.SetBool("isChase", false );
            anim.SetBool("isCritical", false );
        }

        if ((transform.position - playerTransform.position).sqrMagnitude <= playerTransform.GetComponent<Player>().eyeRange)
        {
            EventHandler.CallEnemyInAttackListEvent(this.gameObject, true);
        }
        else
        {
            EventHandler.CallEnemyInAttackListEvent(this.gameObject, false);
        }

        if (dizzytime > 0)
        {
            anim.SetBool("isDizzy", true);
            anim.SetBool("isMoving", false);
            anim.SetBool("isChase", false);
            dizzytime -= Time.deltaTime;
        }
    }

    private void SwitchAnimation()
    {
        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isChase", isChase);
        anim.SetBool("isCritical", enemyInformation.isCritical);
        anim.SetBool("Dead", isDead);

        if (enemyInformation.CheckIsFatal(enemyInformation.CurrentWound, enemyInformation.MaxWound))
            anim.SetTrigger("isFatal");
    }

    /// <summary>
    /// Switch the states of the NPC and Enemy accoridng to the action done by the player
    /// </summary>
    private void SwitchStates()
    {
        if (isDead)
            npcDetails.enemyState = EnemyState.死亡;
        else if (FindPlayer())
        {
            if (transform.CompareTag("Enemy"))
            {
                npcDetails.enemyState = EnemyState.攻击;
                
            }
            else if (transform.CompareTag("NPC"))
            {
                npcDetails.enemyState = EnemyState.和平;
            }
        }
            

        switch (npcDetails.enemyState)
        {
            case EnemyState.和平:
                isMoving = true;
                remainLookAtTime = lookAtTime;
                if (remainLookAtTime > 0)
                {
                    remainLookAtTime -= Time.deltaTime;
                    ChaseTheEnemy(originalPos);
                }

                if (Vector3.SqrMagnitude(originalPos - transform.position) <= Settings.stopDistance)
                {
                    isMoving = false;
                    transform.localScale = new Vector3(direction, 1, 1);
                }

                break;
            case EnemyState.警惕:
                break;
            case EnemyState.攻击:
                isMoving = false;
                speed = enemyInformation.Speed;
                if (!FindPlayer())
                {
                    isChase = false;  
                    //TODO：后期加上警惕的动作
                    if (remainLookAtTime > 0)
                    {
                        remainLookAtTime -= Time.deltaTime;
                    }
                    else if (isPeace)
                        npcDetails.enemyState = EnemyState.和平;
                    else
                        npcDetails.enemyState = EnemyState.巡逻;
                }
                else
                {
                    isChase = true;
                    
                    if (attackTarget!=null)
                    {
                        ChaseTheEnemy(attackTarget.transform.position);
                    }
                    
                }
                
                EnemyAttackSelection();


                break;
            case EnemyState.巡逻:
                isChase = false;
                speed = enemyInformation.Speed/2;
                if (isObstacle)
                {
                    GenerateWayPoint();
                    isObstacle = false;
                    return;
                }
                
                if (Vector3.Distance(wayPoint, transform.position) <= Settings.stopDistance)
                {
                    isMoving = false;
                    if (remainLookAtTime > 0)
                        remainLookAtTime -= Time.deltaTime;
                    
                    else
                        GenerateWayPoint();
                }
                else
                {
                    isMoving = true;
                    ChaseTheEnemy(wayPoint);
                }
                break;
            case EnemyState.死亡:
                coll.enabled = false;
                if (GetComponent<LootSpawner>() != null)
                {
                    GetComponent<LootSpawner>().InitLootItem((int)npcDetails.enemyType, npcDetails.NPCBag);
                }
                Destroy(gameObject, 2f);
                break;
        }
    }


    #region EnemyAction
    /// <summary>
    /// Use the possibility of the skill to determine the next attack action
    /// </summary>
    private void EnemyAttackSelection()
    {
        
        //FIXME:后期加上如果总概率超过100%要怎么办
        if(timeAfterSkillSpell<=0 && canAttack)
        {
            foreach(SkillDetails_SO skill in GetComponent<Skills>().skillList)
            {
                if (skill.currentCoolDown <= 0)
                {
                    StartCoroutine(EnemySkillSeletion());
                    return;
                }
            }
        }
        
        if(attackCoolingTime<=0 && canAttack)
        {

            StartCoroutine(Attack());
        }

        
    }

    private IEnumerator Attack()
    {
        if (TargetInMeleeRange())
        {
            isChase = false;
            if (canAttack)
            {
                enemyInformation.isCritical = Random.value < (enemyInformation.CriticalPoint / Settings.criticalConstant);
                enemyInformation.isConDamage = Random.value < enemyInformation.Continuous_DamageRate;
                anim.SetTrigger("isAttack");
                yield return null;
                RefreshAttackTime();
            }

        }
        //TODO:等到第一个有远程攻击方式的怪物完成
        //if (TargetInRangedRange())
        //{

        //}
    }


    private IEnumerator EnemySkillSeletion()
    {
        float currentValue=Random.value;
        foreach (SkillDetails_SO skill in GetComponent<Skills>().skillList)
        {
            if (skill != null)
            {
                if (skill.QiComsume <= enemyInformation.CurrentQi && TargetInSkillRange(skill.skillRange))
                {
                    currentValue -= skill.skillProbability;
                    if (currentValue <= 0)
                    {
                        isChase = false;
                        anim.SetTrigger("Skill");
                        skill.skillPrefab.GetComponent<SkillSpeller>().CallTheSkill(skill, attackTarget, transform.gameObject);
                        yield return null;
                        timeAfterSkillSpell = skill.timeAfterSpell;
                    }
                }
            }
        }
        yield return null;

    }
    #endregion

    #region Cheking Process
    /// <summary>
    /// Check whether this enemy should occur in this scene.If not, set it inactive
    /// </summary>
    private void CheckVisible()
    {
        if (transform.CompareTag("Enemy"))
        {
            if (initialScene == UnityEngine.SceneManagement.SceneManager.GetActiveScene().name)
                transform.gameObject.SetActive(true);
            else
                transform.gameObject.SetActive(false);
        }
    }

    #endregion
    /// <summary>
    /// Find all the colliders in the circular area and recongise the tag of each colliders
    /// </summary>
    /// <returns></returns>
    private bool FindPlayer()
    {
        var colliders = Physics2D.OverlapCircleAll(transform.position, sightRadius);
        foreach(var target in colliders)
        {
            if (target.CompareTag("Player"))
            {
                attackTarget = target.gameObject;
                return true;
            }
        }
        attackTarget = null;
        return false;

    }

    /// <summary>
    /// Chase the enemy
    /// </summary>
    /// <param name="target">Target</param>
    private void ChaseTheEnemy(Vector3 targetPos)
    {
        if (!canAttack || !canAction) return;

        float distance = (transform.position - targetPos).sqrMagnitude;
        
        if (distance > Settings.stopDistance)//CheckStopDistance())
        {
            int faceDir = (int)transform.localScale.x;

            if (transform.position.x - targetPos.x > 0)
                    faceDir = 1;
            if (transform.position.x - targetPos.x < 0)
                    faceDir = -1;


            transform.localScale = new Vector3(faceDir, 1, 1);
            transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);           
        }
        else
        {
            isChase = false;
        }
        

    }
    #region Events
    public void RefreshAttackTime()
    {
        attackCoolingTime = enemyInformation.AttackCooling;
    }
    public void SetAttackTime(float time)
    {
        attackCoolingTime = time;
    }
    public void StopAttackMoving()
    {
        transform.position = attackPos;
    }

    public void UpdateAttackPos(Vector3 targetPos)
    {
        attackPos = targetPos;
    }
    #endregion
    /// <summary>
    /// Generate a next position after check it is available
    /// </summary>
    private void GenerateWayPoint()
    {
        remainLookAtTime = lookAtTime;
        float randomX = Random.Range(-PatrolRange, PatrolRange);
        float randomY = Random.Range(-PatrolRange, PatrolRange);

        Vector3 randomPoint = new Vector3(originalPos.x + randomX, originalPos.y+randomY, transform.position.z);

        TileDetails targetTile = GridMapManager.Instance.GetTileDetailsAtTargetPosition(randomPoint);
        if (targetTile != null)
        {
            if (!targetTile.isNPCObstacle)
                wayPoint = randomPoint;
            else
                isObstacle = true;
                return;
        }
        else
        {
            wayPoint = randomPoint;
        }
        
    }
    #region CheckRange
    protected bool TargetInMeleeRange()
    {
        if (attackTarget != null)
            return Vector3.Distance(attackTarget.transform.position, transform.position) <= enemyInformation.MinimumRange;
        else
            return false;
    }

    protected bool TargetInRangedRange()
    {
        if (attackTarget != null)
            return Vector3.Distance(attackTarget.transform.position, transform.position) <= enemyInformation.MaximumRange;
        else
            return false;
    }
    protected bool TargetInSkillRange(float skillRange)
    {
        if (attackTarget != null)
            return Vector3.Distance(attackTarget.transform.position, transform.position) <= skillRange;
        else
            return false;
    }
    #endregion
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, PatrolRange);
        if (enemyInformation != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, enemyInformation.MinimumRange);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, enemyInformation.MaximumRange);
        }
        
    }

    protected virtual void Hit()
    { 
        if (attackTarget != null )
        {
            if (attackTarget.CompareTag("Player"))
            {
                if (attackTarget.GetComponent<Player>().isParry)
                {
                    EventHandler.CallDamageTextPopEvent(attackTarget.transform, 0, AttackEffectType.Undefeated);
                    dizzytime = Settings.parryDizzyTime;
                    return;
                }
            }
            var targetInformation = attackTarget.GetComponent<CharacterInformation>();

            targetInformation.FinalDamage(enemyInformation, targetInformation);
            if (attackTarget.CompareTag("Player"))
            {
                attackTarget.GetComponent<CharacterInformation>().healthChange?.Invoke(attackTarget.GetComponent<CharacterInformation>());
            }

        }
    }

    public void EndNotify()
    {
        isChase = false;
        playerDead = true;
        isMoving = false;
        attackTarget = null;
    }
}
