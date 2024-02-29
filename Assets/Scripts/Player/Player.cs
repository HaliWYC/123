using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Skills))]
public class Player : MonoBehaviour
{
    public Rigidbody2D rb;
    private Animator anim;
    public PlayerInputControl inputControl;
    private bool isMoving;
    private bool inputDisable;

    private CharacterInformation playerInformation;

    [SerializeField] private Transform playerTransform;

    public float speed;
    //private float inputX;
    //private float inputY;

    public Vector2 movementInput;

    //Player Attack
    private float lastAttackTime;
    private float lastParryTime;
    private float parryDuration=0.2f;
    private float currentParryDuration;
    private GameObject attackTarget;
    public bool isAttackEnd;
    private bool isAttack;
    public bool isParry;
    private bool isDead;

    private void Awake()
    {
        
        inputControl = new PlayerInputControl();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        playerInformation=GetComponent<CharacterInformation>();
        isAttackEnd = true;
        //Debug.Log("Player");
    }

    private void Start()
    {
        //FIXME Use in load the scene
        GameManager.Instance.RegisterPlayer(playerInformation);
        playerInformation.healthChange?.Invoke(playerInformation);
        playerInformation.qiChange?.Invoke(playerInformation);
        playerInformation.woundChange?.Invoke(playerInformation);
        playerInformation.moraleChange?.Invoke(playerInformation);
    }

    private void Update()
    {
        isDead = playerInformation.CurrentHealth == 0;
        if (isDead)
        {
            StopPLayerInput();
            GameManager.Instance.NotifyObservers();
        }

        //PlayerInput();
        if (inputDisable == true)
        {
            inputControl.Disable();
            //isMoving = false;
        }
        else
            inputControl.Enable();

        lastAttackTime -= Time.deltaTime;
        lastParryTime -= Time.deltaTime;
        PlayerAttack();
        PlayerParry();
        movementInput = inputControl.Gameplay.Move.ReadValue<Vector2>();
        
        SwitchAnimation();
        Running();
        if (playerInformation.isUndefeated)
        {
            currentParryDuration -= Time.deltaTime;
            if (currentParryDuration <= 0)
                playerInformation.SetUndefeated(false);
        }

    }

    private void FixedUpdate()
    {
        //if(!inputDisable)
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
    }

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
                break;
            case GameState.Pause:
                inputDisable = true;
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

    

    /*private void PlayerInput()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");

        //This let the speed of player to move diagonally at the same speed as it moves horizontally or vertically
        if(inputX != 0 && inputY != 0)
        {
            inputX *= 0.7f;
            inputY *= 0.7f;
        }

        movementInput = new Vector2(inputX, inputY);

    }*/

    private void Movement()
    {
        //rb.MovePosition(rb.position + movementInput * speed * Time.deltaTime);
        if (movementInput.x != 0 && movementInput.y != 0)
        {
            movementInput.x *= 0.8f;
            movementInput.y *= 0.8f;
        }
        //Debug.Log(movementInput.x + movementInput.y);
        rb.velocity = new Vector2(movementInput.x * speed * Time.deltaTime, movementInput.y * speed * Time.deltaTime);
        //Debug.Log(rb.velocity.x + rb.velocity.y);
        //Debug.Log(movementInput.x + movementInput.y);
        isMoving = rb.velocity != Vector2.zero;
        //Flip player

        int faceDir = (int)playerTransform.localScale.x;

        if (movementInput.x > 0)
            faceDir = 1;
        if (movementInput.x < 0)
            faceDir = -1;


        playerTransform.localScale = new Vector3(faceDir, 1, 1);

    }

    private void Running()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = playerInformation.Speed * 2;
        }
        else
        {
            speed = playerInformation.Speed;
        }
        
    }

    public void StopPLayerInput()
    {
        inputDisable = true;
    }

    public void AllowPLayerInput()
    {
        inputDisable = false;
    }
    public void SwitchAnimation()
    {
        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isCritical", playerInformation.isCritical);
        anim.SetBool("Dead", isDead);
        anim.SetBool("isAttack", isAttack);
        if (isMoving)
        {
            anim.SetFloat("velocityX", rb.velocity.x);
            anim.SetFloat("velocityY", rb.velocity.y);
        }
        if (playerInformation.CheckIsFatal(playerInformation.CurrentWound, playerInformation.MaxWound))
            anim.SetTrigger("Fatal");
    }

    public void PlayerAttack()
    {
        if (isDead) return;
        if (lastAttackTime < 0 && CursorManager.Instance.cursorPositionValid && Input.GetMouseButtonUp(0))
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
            isAttack = true;
        }
    }

    public void PlayerParry()
    {
        if (isDead) return;
        if(lastParryTime<0 && Input.GetMouseButtonDown(1))
        {
            lastParryTime = playerInformation.ParryCoolDown;
            isParry = true;
            anim.SetTrigger("Parry");
        }
    }

    public void ParryEffect()
    {
        if (!playerInformation.isUndefeated)
        {
            playerInformation.SetUndefeated(true);
            currentParryDuration = parryDuration;
        }

    }

    private void OnTriggerEnter2D(Collider2D target)
    {
        if (target.CompareTag("NPC") || target.CompareTag("Enemy"))
            attackTarget = target.gameObject;
        else
            return;
    }
    private void OnTriggerExit2D(Collider2D target)
    {
        attackTarget = null;
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

}
