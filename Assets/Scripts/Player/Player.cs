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
    private Skills skills;

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
        skills = GetComponent<Skills>();
        isAttackEnd = true;
        isRolling = false;
        canAction = true;
        canExecute = false;
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
        EventHandler.UpdatePlayerStateEvent += OnUpdatePlayerStateEvent;
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
        EventHandler.UpdatePlayerStateEvent -= OnUpdatePlayerStateEvent;
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
                {
                    EventHandler.CallUpdatePlayerStateEvent(PlayerState.战斗);
                }

                if (Input.GetKeyDown(KeyCode.C))
                {
                    playerState = PlayerState.潜行;
                }
                break;
            case PlayerState.战斗:
                //TODO:Play combat Animation
                if (Input.GetKeyDown(KeyCode.J))
                {
                    EventHandler.CallUpdatePlayerStateEvent(PlayerState.和平);
                }
                SelectEnemyFromList();
                PlayerSkillSpelling();
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
        UpdateEnemyInList();
    }

    

    private void FixedUpdate()
    {

        Movement();
    }

    

    #region Events

    private void OnAllowPlayerInputEvent(bool input)
    {
        inputDisable = !input;
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

    private void OnUpdatePlayerStateEvent(PlayerState state)
    {
        playerState = state;
        switch (playerState)
        {
            case PlayerState.和平:
                PlayerStateUI.Instance.playerState.SetActive(false);
                break;
            case PlayerState.战斗:
                PlayerStateUI.Instance.playerState.SetActive(true);
                break;
        }
    }

    #endregion

    #region PlayerCheck

    private bool CheckEnemyInList(GameObject target)
    {
        foreach(var enemy in attackTargetList)
        {
            if (enemy == target)
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
        if(lastParryTime<=0 && Input.GetKeyDown(KeyCode.H))
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
        CharacterInformation enemy = attackTarget.GetComponent<CharacterInformation>();
        enemy.CurrentWound += enemy.MaxWound;
        StartCoroutine(enemy.CalculateFatal(playerInformation.Fatal_Enhancement, enemy.FatalDefense, enemy));
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
            }
        }
    }

    private void UpdateEnemyInList()
    {
        var collider = Physics2D.OverlapCircleAll(transform.position, eyeRange);
        attackTargetList.Clear();
        foreach(var enemy in collider)
        {
            if (enemy.CompareTag("Enemy") || enemy.CompareTag("NPC"))
            {
                attackTargetList.Add(enemy.gameObject);
            }
        }
    }

    private void PlayerSkillSpelling()
    {
        if (isDead || isExecute) return;
        if (Input.GetKeyDown(KeyCode.Alpha1) && skills.skillList[0] != null)
        {
            if (attackTarget != null && skills.skillList[0].currentCoolDown <= 0)
                SpellSkill(skills.skillList[0]);
                
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && skills.skillList[1] != null)
        {
            if (attackTarget != null && skills.skillList[1].currentCoolDown <= 0)
                SpellSkill(skills.skillList[1]);

        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && skills.skillList[2] != null)
        {
            if (attackTarget != null && skills.skillList[2].currentCoolDown <= 0)
                SpellSkill(skills.skillList[2]);

        }
        if (Input.GetKeyDown(KeyCode.Alpha4) && skills.skillList[3] != null)
        {
            if (attackTarget != null && skills.skillList[3].currentCoolDown <= 0)
                SpellSkill(skills.skillList[3]);

        }
        if (Input.GetKeyDown(KeyCode.Alpha5) && skills.skillList[4] != null)
        {
            if (attackTarget != null && skills.skillList[4].currentCoolDown <= 0)
                SpellSkill(skills.skillList[4]);

        }
        if (Input.GetKeyDown(KeyCode.Alpha6) && skills.skillList[5] != null)
        {
            if (attackTarget != null && skills.skillList[5].currentCoolDown <= 0)
                SpellSkill(skills.skillList[5]);

        }
        if (Input.GetKeyDown(KeyCode.Alpha7) && skills.skillList[6] != null)
        {
            if (attackTarget != null && skills.skillList[6].currentCoolDown <= 0)
                SpellSkill(skills.skillList[6]);

        }
        if (Input.GetKeyDown(KeyCode.Alpha8) && skills.skillList[7] != null)
        {
            if (attackTarget != null && skills.skillList[7].currentCoolDown <= 0)
                SpellSkill(skills.skillList[7]);

        }
        if (Input.GetKeyDown(KeyCode.Alpha9) && skills.skillList[8] != null)
        {
            if (attackTarget != null && skills.skillList[8].currentCoolDown <= 0)
                SpellSkill(skills.skillList[8]);

        }
        if (Input.GetKeyDown(KeyCode.Alpha0) && skills.skillList[9] != null)
        {
            if (attackTarget != null && skills.skillList[9].currentCoolDown <= 0)
                SpellSkill(skills.skillList[9]);

        }

    }

    private void SpellSkill(SkillDetails_SO skill)
    {
        skill.skillPrefab.GetComponent<SkillSpeller>().CallTheSkill(skill, attackTarget, gameObject);
    }

    #endregion


    private void OnTriggerEnter2D(Collider2D target)
    {
        if (target.CompareTag("Enemy") || target.CompareTag("NPC"))
        {
            attackTarget = target.gameObject;
            if (!CheckEnemyInList(attackTarget))
            {
                attackTargetList.Add(attackTarget);
            }
        }
    }


    //private void OnTriggerExit2D(Collider2D target)
    //{
    //    attackTarget = null;
    //}

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
