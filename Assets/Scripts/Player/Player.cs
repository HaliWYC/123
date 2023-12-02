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
        GameManager.Instance.registerPlayer(playerInformation);
    }

    private void Update()
    {
        isDead = playerInformation.CurrentHealth == 0;
        if (isDead)
        {
            stopPLayerInput();
            GameManager.Instance.notifyObservers();
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
        playerAttack();
        playerParry();
        movementInput = inputControl.Gameplay.Move.ReadValue<Vector2>();
        
        switchAnimation();
        Running();
        if (playerInformation.isUndefeated)
        {
            currentParryDuration -= Time.deltaTime;
            if (currentParryDuration <= 0)
                playerInformation.setUndefeated(false);
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
        EventHandler.beforeSceneUnloadEvent += onBeforeSceneUnloadEvent;
        EventHandler.afterSceneLoadedEvent += onAfterSceneLoadEvent;
        EventHandler.mouseClickEvent += onMouseClickEvent;
        EventHandler.moveToPosition += onMoveToPosition;
        EventHandler.updateGameStateEvent += onUpdateGameStateEvent;
        EventHandler.allowPlayerInputEvent += onAllowPlayerInputEvent;
    }

    private void OnDisable()
    {
        inputControl.Disable();
        EventHandler.beforeSceneUnloadEvent -= onBeforeSceneUnloadEvent;
        EventHandler.afterSceneLoadedEvent -= onAfterSceneLoadEvent;
        EventHandler.moveToPosition -= onMoveToPosition;
        EventHandler.mouseClickEvent -= onMouseClickEvent;
        EventHandler.updateGameStateEvent -= onUpdateGameStateEvent;
        EventHandler.allowPlayerInputEvent -= onAllowPlayerInputEvent;
    }

    private void onAllowPlayerInputEvent(bool input)
    {
        inputDisable = !input;
    }

    private void onUpdateGameStateEvent(GameState gameState)
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

    private void onMouseClickEvent(Vector3 pos, ItemDetails itemDetails)
    {
        //TODO: Execute animation

        EventHandler.callExecuteActionAfterAnimation(pos, itemDetails);
    }

    private void onMoveToPosition(Vector3 targetPosition)
    {
        transform.position = targetPosition;
    }

    private void onAfterSceneLoadEvent()
    {
        inputDisable = false;
    }

    private void onBeforeSceneUnloadEvent()
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

    public void stopPLayerInput()
    {
        inputDisable = true;
    }

    public void allowPLayerInput()
    {
        inputDisable = false;
    }
    public void switchAnimation()
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
        if (playerInformation.checkIsFatal(playerInformation.CurrentWound, playerInformation.MaxWound))
            anim.SetTrigger("Fatal");
    }

    public void playerAttack()
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

    public void playerParry()
    {
        if (isDead) return;
        if(lastParryTime<0 && Input.GetMouseButtonDown(1))
        {
            lastParryTime = playerInformation.ParryCoolDown;
            isParry = true;
            anim.SetTrigger("Parry");
        }
    }

    public void parryEffect()
    {
        if (!playerInformation.isUndefeated)
        {
            playerInformation.setUndefeated(true);
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

    public void hit()
    {
        if (attackTarget.GetComponent<CharacterInformation>().isUndefeated)
        {
            Debug.Log("Undefeated");
            return;
        }
        
        if (attackTarget != null)
        {
            //if (transform.isFacingTarget(attackTarget.transform))
            //{
                var targetInformation = attackTarget.GetComponent<CharacterInformation>();
                targetInformation.finalDamage(playerInformation, targetInformation);
            //}
        }
        
    }

}
