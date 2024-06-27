using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using ShanHai_IsolatedCity.Skill;

[RequireComponent(typeof(Skills))]
public class Player : MonoBehaviour
{
    public Rigidbody2D rb;
    private Animator anim;
    public PlayerInputControl inputControl;
    private bool inputDisable;
    private PlayerState playerState;
    public CharacterInformation playerInformation;

    [SerializeField] private Transform playerTransform;

    public float speed;

    public Vector2 movementInput;

    //Player Attack
    public float eyeRange;
    private float lastAttackTime;
    private float lastParryTime;
    private float runningComsumeLimit;
    private int runnerComsume = 5;
    public List<GameObject> attackTargetList;
    public GameObject attackTarget;

    public bool canExecute;
    private bool canAction;
    private bool isMoving;
    public bool isAttackEnd;
    private bool isDead;
    private bool isRolling;
    public bool isExecute;
    public bool isParry;

    private void Awake()
    {
        
        inputControl = new PlayerInputControl();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        playerInformation=GetComponent<CharacterInformation>();
        isAttackEnd = true;
        isRolling = false;
        canAction = true;
        canExecute = false;
    }

    private void Start()
    {
        //FIXME Use in load the scene
        GameManager.Instance.RegisterPlayer(playerInformation);
        playerInformation.healthChange?.Invoke(playerInformation);
        playerInformation.qiChange?.Invoke(playerInformation);
        playerInformation.vigorChange?.Invoke(playerInformation);
        playerInformation.woundChange?.Invoke(playerInformation);
        playerInformation.moraleChange?.Invoke(playerInformation);
        playerState = PlayerState.和平;
    }

    private void Update()
    {
        isDead = playerInformation.CurrentHealth <= 0;
        eyeRange = CheckEyeRange();
        if (isDead)
        {
            inputDisable = true;
            GameManager.Instance.NotifyObservers();
        }
        if (inputDisable == true)
            inputControl.Disable();
        else
            inputControl.Enable();
        lastAttackTime -= Time.deltaTime;
        lastParryTime -= Time.deltaTime;

        switch (playerState)
        {
            case PlayerState.和平:
                if (Input.GetKeyDown(KeyCode.J))
                    playerState = PlayerState.战斗;
                if (Input.GetKeyDown(KeyCode.C))
                {
                    playerState = PlayerState.潜行;
                    Debug.Log(playerState);
                }

                break;
            case PlayerState.战斗:
                //TODO:Play combat Animation
                if (Input.GetKeyDown(KeyCode.J))
                    playerState = PlayerState.和平;
                SelectEnemyFromList();
                PlayerExecute();
                PlayerParry();
                PlayerAttack();
                break;
            case PlayerState.潜行:
                //TODO:Play sqaut Animation
                if (Input.GetKeyDown(KeyCode.C))
                {
                    playerState = PlayerState.和平;
                    Debug.Log(playerState);
                }
                break;
        }
        PlayerRolling();
        PlayerRunning();
        movementInput = inputControl.Gameplay.Move.ReadValue<Vector2>();
        SwitchAnimation();
    }

    

    private void FixedUpdate()
    {

        Movement();
    }

    private void OnEnable()
    {
        inputControl.Enable();
        EventHandler.BeforeSceneUnloadEvent += OnBeforeSceneUnloadEvent;
        EventHandler.AfterSceneLoadedEvent += OnAfterSceneLoadEvent;
        EventHandler.MouseClickEvent += OnMouseClickEvent;
        EventHandler.MoveToPosition += OnMoveToPosition;
        EventHandler.UpdateGameStateEvent += OnUpdateGameStateEvent;
        EventHandler.AllowPlayerInputEvent += OnAllowPlayerInputEvent;
        EventHandler.EnemyInAttackListEvent += OnEnemyInAttackListEvent;
    }

    private void OnDisable()
    {
        inputControl.Disable();
        EventHandler.BeforeSceneUnloadEvent -= OnBeforeSceneUnloadEvent;
        EventHandler.AfterSceneLoadedEvent -= OnAfterSceneLoadEvent;
        EventHandler.MoveToPosition -= OnMoveToPosition;
        EventHandler.MouseClickEvent -= OnMouseClickEvent;
        EventHandler.UpdateGameStateEvent -= OnUpdateGameStateEvent;
        EventHandler.AllowPlayerInputEvent -= OnAllowPlayerInputEvent;
        EventHandler.EnemyInAttackListEvent -= OnEnemyInAttackListEvent;
    }

    #region Events

    private void OnAllowPlayerInputEvent(bool input)
    {
        inputDisable = !input;
    }

    private void OnEnemyInAttackListEvent(GameObject enemy, bool shouldExist)
    {
        if (shouldExist)
        {
           if(!CheckEnemyExist(attackTargetList,enemy))
            attackTargetList.Add(enemy);
        }
        else if(attackTargetList.Count>0 && CheckEnemyExist(attackTargetList, enemy))
            attackTargetList.RemoveAt(attackTargetList.IndexOf(enemy));
    }

    private void OnUpdateGameStateEvent(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.GamePlay:
                inputDisable = false;
                canAction = true;
                break;
            case GameState.Pause:
                inputDisable = true;
                canAction = false;
                break;
        }
    }

    private void OnMouseClickEvent(Vector3 pos, ItemDetails itemDetails)
    {
        //TODO: Execute animation
        EventHandler.CallExecuteActionAfterAnimation(pos, itemDetails);
    }

    private void OnMoveToPosition(Vector3 targetPosition)
    {
        transform.position = targetPosition;
    }

    private void OnAfterSceneLoadEvent()
    {
        inputDisable = false;
    }

    private void OnBeforeSceneUnloadEvent()
    {
        inputDisable = true;
    }

    #endregion

    #region PlayerCheck
    /// <summary>
    /// Check whether the given target has already exist in the attackTarget List
    /// </summary>
    /// <param name="attackList"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    private bool CheckEnemyExist(List<GameObject> attackList, GameObject target)
    {
        foreach (GameObject enemy in attackList)
        {
            if (target == enemy)
                return true;
        }
        return false;
    }

    private float CheckEyeRange()
    {
        return Mathf.Max(playerInformation.MinimumRange, playerInformation.MaximumRange) + Settings.eyeRangeBase;
    }

    private void FaceAttackTarget()
    {
        if(attackTarget!=null)
        if (attackTarget.transform.position.x >= transform.position.x)
        {
            if (playerTransform.localScale.x >= 0)
                transform.position = new Vector3(attackTarget.transform.position.x - Settings.stopDistance, attackTarget.transform.position.y, attackTarget.transform.position.z);
            else
                transform.position = new Vector3(attackTarget.transform.position.x + Settings.stopDistance, attackTarget.transform.position.y, attackTarget.transform.position.z);
        }
        else
        {
            if (playerTransform.localScale.x >= 0)
                transform.position = new Vector3(attackTarget.transform.position.x - Settings.stopDistance, attackTarget.transform.position.y, attackTarget.transform.position.z);
            else
                transform.position = new Vector3(attackTarget.transform.position.x + Settings.stopDistance, attackTarget.transform.position.y, attackTarget.transform.position.z);
        }

    }

    #endregion



    private void Movement()
    {
        if (movementInput.x != 0 && movementInput.y != 0)
        {
            movementInput.x *= 0.8f;
            movementInput.y *= 0.8f;
        }
        rb.velocity = new Vector2(movementInput.x * speed * Time.deltaTime, movementInput.y * speed * Time.deltaTime);
        isMoving = rb.velocity != Vector2.zero;


        //Flip player
        int faceDir = (int)playerTransform.localScale.x;

        if (movementInput.x > 0)
            faceDir = 1;
        if (movementInput.x < 0)
            faceDir = -1;


        playerTransform.localScale = new Vector3(faceDir, 1, 1);

    }
    public void SwitchAnimation()
    {
        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isCritical", playerInformation.isCritical);
        anim.SetBool("Dead", isDead);
        anim.SetBool("canExecute", !isExecute);
        if (isMoving)
        {
            anim.SetFloat("velocityX", rb.velocity.x);
            anim.SetFloat("velocityY", rb.velocity.y);
        }
        if (playerInformation.CheckIsFatal(playerInformation.CurrentWound, playerInformation.MaxWound))
            anim.SetTrigger("isFatal");
    }

    #region PlayerAction
    public void PlayerAttack()
    {
        if (isDead || !canAction || isExecute || isRolling || isParry) return;

        if (lastAttackTime < 0 /*&& CursorManager.Instance.cursorPositionValid*/ && Input.GetMouseButtonUp(0))
        {
            if (attackTarget == null)
            {
                playerInformation.isCritical = false;
            }
            lastAttackTime = playerInformation.AttackCooling;
            if (isAttackEnd && attackTarget!=null)
            {
                playerInformation.isCritical = UnityEngine.Random.value < (playerInformation.CriticalPoint / Settings.criticalConstant);
                playerInformation.isConDamage = UnityEngine.Random.value < (playerInformation.Continuous_DamageRate);
            }
            anim.SetTrigger("Attack");
        }
    }

    private void PlayerRunning()
    {
        if (isDead || !canAction || isExecute || isRolling) return;
        if (Input.GetKey(KeyCode.LeftShift) && playerInformation.CurrentVigor-runnerComsume > 0)
        {
            speed = playerInformation.Speed * 2;
            if (rb.velocity.SqrMagnitude() != 0)
            {
                //playerInformation.CurrentVigor = Mathf.Max(playerInformation.CurrentVigor - runnerComsume, 0);
                playerInformation.CurrentVigor -= runnerComsume;
                playerInformation.vigorChange.Invoke(playerInformation);
                runningComsumeLimit -= Time.deltaTime;
                if (runningComsumeLimit < 0)
                {
                    runnerComsume += 5;
                    runningComsumeLimit = Settings.runningComsumeLevelLimit;
                }
            }
        }
        else
        {
            speed = playerInformation.Speed;
            runningComsumeLimit = Settings.runningComsumeLevelLimit;
            runnerComsume = 5;
        }

    }
    public void PlayerParry()
    {
        if (isDead || !canAction || isExecute || isRolling || !isAttackEnd) return;
        if(lastParryTime<=0 && Input.GetMouseButtonDown(1))
        {
            anim.SetTrigger("isParry");
        }
    }

    public void StartParry()
    {
        playerInformation.SetUndefeated(true);
        isParry = true;
    }

    public void EndParry()
    {
        playerInformation.SetUndefeated(false);
        isParry = false;
        lastParryTime = playerInformation.ParryCoolDown;
    }

    public void PlayerRolling()
    {
        if (isDead || !canAction || isExecute || !isAttackEnd || isParry) return;
        if(Input.GetKeyDown(KeyCode.Space) && playerInformation.CurrentVigor - Settings.rollingComsume > 0)
        {
            anim.SetTrigger("isRolling");
            playerInformation.CurrentVigor -= Settings.rollingComsume;
            playerInformation.vigorChange.Invoke(playerInformation);
        }
    }

    public void StartRolling()
    {
        playerInformation.SetComfirmDodged(true);
        isRolling = true;
    }

    public void EndRolling()
    {
        playerInformation.SetComfirmDodged(false);
        isRolling = false;
    }

    public void PlayerExecute()
    {
        if (isDead || isExecute) return;

        if (Input.GetKeyDown(KeyCode.E) && canExecute && !isExecute)
        {
            anim.SetTrigger("Execute");
        }
    }

    public void StartExecute()
    {
        FaceAttackTarget();
        inputDisable = true;
        playerInformation.isUndefeated = true;
        attackTarget.GetComponent<EnemyController>().dizzytime = 100;
        isExecute = true;
    }

    public void EndExecute()
    {
        inputDisable = false;
        attackTarget.GetComponent<EnemyController>().dizzytime = 0;
        isExecute = false;
        playerInformation.isUndefeated = false;
        playerInformation.ExecuteBenefits();
    }

    public void Hit()
    {
        if (attackTarget != null)
        {
            if (attackTarget.GetComponent<CharacterInformation>().isUndefeated)
            {
                EventHandler.CallDamageTextPopEvent(attackTarget.transform, 0, AttackEffectType.Undefeated);
                return;
            }
            
            var targetInformation = attackTarget.GetComponent<CharacterInformation>();
            targetInformation.FinalDamage(playerInformation, targetInformation);
        }
    }

    /// <summary>
    /// Select a enemy from attackTargetList as the attackTarget
    /// </summary>
    private void SelectEnemyFromList()
    {
        if (Input.GetKeyDown(KeyCode.F) && attackTargetList.Count > 0)
        {
            int indexInList = UnityEngine.Random.Range(0, attackTargetList.Count);

            if (attackTargetList[indexInList] != null)
            {
                attackTarget = attackTargetList[indexInList];
                Debug.Log(attackTarget);
            }
        }
    }
    #endregion


    private void OnTriggerEnter2D(Collider2D target)
    {
        if (target.CompareTag("Enemy") || target.CompareTag("NPC"))
        {
            if(attackTarget==null)
            attackTarget = target.gameObject;
        }
    }


    private void OnTriggerExit2D(Collider2D target)
    {
        attackTarget = null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, eyeRange);
    }

    public void PlayDeath()
    {
        Time.timeScale = 0;
        //TODO:播放死亡界面和后续事件
    }
}
