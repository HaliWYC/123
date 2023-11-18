using ShanHai_IsolatedCity.Map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterInformation))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Skills))]
public class EnemyController : MonoBehaviour,IEndGameObserver
{
    public string Name;

    private NPCDetails npcDetails;

    protected GameObject attackTarget;
    private Collider2D coll;
    protected Animator anim;
    protected CharacterInformation enemyInformation;
    private float speed;

    [Header("状态")]
    public bool isPeace;
    private bool isChase;
    private bool isMoving;
    private bool isDead;
    private bool playerDead;
    [HideInInspector]
    public bool Attack;
    [HideInInspector]
    public bool isAttackEnd;
    protected bool isConAttack;
    [HideInInspector]
    public float conAttackTime;
    private float lastAttackTime;
    [HideInInspector]
    private float direction;

    public bool isSkilling;

    private Vector3 originalPos;
    [HideInInspector]
    public Vector3 attackPos;
    private bool isObstacle;

    
    //private Vector3 stopDistance => new Vector3(new Vector2(Random.Range(-0.2f,0.2f), Random.Range(-0.2f, 0.2f)).sqrMagnitude, new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).sqrMagnitude, 0);

    [Header("巡逻范围")]
    public float PatrolRange;
    //private float sightRadius;
    private Vector3 wayPoint;
    private float remainLookAtTime;
    public float lookAtTime;

    private void Awake()
    {
        
        anim = GetComponent<Animator>();
        coll = GetComponent<BoxCollider2D>();
        remainLookAtTime = lookAtTime;
        enemyInformation = GetComponent<CharacterInformation>();
        direction = transform.GetComponent<Transform>().localScale.x;
        isAttackEnd = true;
        //sightRadius = npcDetails.sightRadius;
    }

   /* private void OnEnable()
    {
        GameManager.Instance.addObserver(this);
    }*/

    private void OnDisable()
    {
        if (!GameManager.isInitialized) return;
        GameManager.Instance.removeObserver(this);
    }
    private void Start()
    {
        npcDetails = NPCManager.Instance.getNPCDetail(Name);
        speed = enemyInformation.Speed;
        originalPos = transform.position;
        isSkilling = false;
        if (isPeace)
        {
            npcDetails.enemyState = EnemyState.和平;
        }
        else
        {
            npcDetails.enemyState = EnemyState.巡逻;
            generateWayPoint();
            
        }

        //FIXEDME:在加载场景的时候使用
        GameManager.Instance.addObserver(this);
    }

    private void Update()
    {
        isDead = enemyInformation.CurrentHealth == 0;
        if (!playerDead)
        {
            switchStates();
            switchAnimation();
            lastAttackTime -= Time.deltaTime;
            isConAttack = conAttackTime > 0;
            if (isConAttack)
            {
                anim.SetTrigger("isAttack");
                conAttackTime -= Time.deltaTime;
            }
        }
    }

    private void switchAnimation()
    {
        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isChase", isChase);
        anim.SetBool("isCritical", enemyInformation.isCritical);
        anim.SetBool("Dead", isDead);
        anim.SetBool("Attack", Attack);
        if (enemyInformation.checkIsFatal(enemyInformation.CurrentWound, enemyInformation.MaxWound))
            anim.SetTrigger("Fatal");
    }

    /// <summary>
    /// Switch the states of the NPC and Enemy accoridng to the action done by the player
    /// </summary>
    private void switchStates()
    {
        if (isDead)
            npcDetails.enemyState = EnemyState.死亡;

        else if (findPlayer())
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
                    chaseTheEnemy(originalPos);
                }

                if (Vector3.SqrMagnitude(originalPos - transform.position) <= Settings.stopDistance)
                {
                    isMoving = false;
                    transform.localScale = new Vector3(direction, npcDetails.SizeScale, 1);
                }

                break;
            case EnemyState.警惕:
                break;
            case EnemyState.攻击:
                isMoving = false;
                speed = enemyInformation.Speed;
                if (!findPlayer())
                {
                    isChase = false;  
                    //TODO：后期加上警惕的动作
                    if (remainLookAtTime > 0)
                    {
                        stopMoving();
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
                        chaseTheEnemy(attackTarget.transform.position);
                    }

                }
                if (targetInMeleeRange())
                {
                    isChase = false;
                    //chaseTheEnemy(transform.position);
                    stopMoving();
                    if (!isSkilling)
                    {
                        if (lastAttackTime < 0)
                        {
                            lastAttackTime = enemyInformation.fightingData.AttackCooling;
                            if (isAttackEnd)
                            {
                                isAttackEnd = false;
                                Attack = true;
                                enemyInformation.isCritical = Random.value < (enemyInformation.CriticalPoint / Settings.criticalConstant);
                                enemyInformation.isConDamage = Random.value < enemyInformation.Continuous_DamageRate;
                            }
                            attack();
                        }
                    }
                }


                break;
            case EnemyState.巡逻:
                isChase = false;
                speed = enemyInformation.Speed/2;
                if (isObstacle)
                {
                    generateWayPoint();
                    isObstacle = false;
                    return;
                }
                
                if (Vector3.Distance(wayPoint, transform.position) <= Settings.stopDistance)
                {
                    isMoving = false;
                    if (remainLookAtTime > 0)
                        remainLookAtTime -= Time.deltaTime;
                    
                    else
                        generateWayPoint();
                }
                else
                {
                    isMoving = true;
                    chaseTheEnemy(wayPoint);
                }
                break;
            case EnemyState.死亡:
                coll.enabled = false;
                Destroy(gameObject, 2f);
                break;
        }
    }

    private void attack()
    {
        if (targetInMeleeRange())
        {
            
            anim.SetTrigger("isAttack");

        }
        if (targetInRangedRange())
        {

        }
    }


    /// <summary>
    /// Find all the colliders in the circular area and recongise the tag of each colliders
    /// </summary>
    /// <returns></returns>
    private bool findPlayer()
    {
        var colliders = Physics2D.OverlapCircleAll(transform.position, npcDetails.sightRadius);

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
    private void chaseTheEnemy(Vector3 targetPos)
    {
        float distance = (transform.position - targetPos).sqrMagnitude;
        
        if (distance >Settings.stopDistance)
        {
            int faceDir = (int)transform.localScale.x;

            if (transform.position.x - targetPos.x > 0)
                    faceDir = npcDetails.SizeScale;
            if (transform.position.x - targetPos.x < 0)
                    faceDir = -npcDetails.SizeScale;


            transform.localScale = new Vector3(faceDir, npcDetails.SizeScale, 1);
            transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);           
        }
        

    }

    private void stopMoving()
    {
        transform.position = transform.position;
    }

    public void stopAttackMoving()
    {
        transform.position = attackPos;
    }

    public void updateAttackPos(Vector3 targetPos)
    {
        attackPos = targetPos;
    }

    /// <summary>
    /// Generate a way point after check it is available
    /// </summary>
    private void generateWayPoint()
    {
        remainLookAtTime = lookAtTime;
        float randomX = Random.Range(-PatrolRange, PatrolRange);
        float randomY = Random.Range(-PatrolRange, PatrolRange);

        Vector3 randomPoint = new Vector3(originalPos.x + randomX, originalPos.y+randomY, transform.position.z);

        TileDetails targetTile = GridMapManager.Instance.getTileDetailsAtTargetPosition(randomPoint);
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

    private bool targetInMeleeRange()
    {
        if (attackTarget != null)
            return Vector3.Distance(attackTarget.transform.position, transform.position) <= enemyInformation.MeleeRange;
        else
            return false;
    }

    private bool targetInRangedRange()
    {
        if (attackTarget != null)
            return Vector3.Distance(attackTarget.transform.position, transform.position) <= enemyInformation.RangedRange;
        else
            return false;
    }
    private bool targetInSkillRange(float skillRange)
    {
        return Vector3.Distance(attackTarget.transform.position, transform.position) <= skillRange;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, PatrolRange);
    }

    protected virtual void hit()
    {
        
        if (attackTarget != null )
        {
            if(transform.isFacingTarget(attackTarget.transform))
            {
                var targetInformation = attackTarget.GetComponent<CharacterInformation>();
                targetInformation.finalDamage(enemyInformation, targetInformation);
            }
        }
    }

    public void endNotify()
    {
        isChase = false;
        playerDead = true;
        isMoving = false;
        attackTarget = null;
    }
}
