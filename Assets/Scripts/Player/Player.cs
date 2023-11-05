using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;
    private Animator anim;
    public PlayerInputControl inputControl;
    private bool isMoving;
    private bool inputDisable;

    [SerializeField] private Transform playerTransform;

    public float speed;
    //private float inputX;
    //private float inputY;

    public Vector2 movementInput;




    private void Awake()
    {
        inputControl = new PlayerInputControl();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        //Debug.Log("Player");
    }

    

    private void Update()
    {
        //PlayerInput();
        if (inputDisable == true)
        {
            inputControl.Disable();
            //isMoving = false;
        }
        else
            inputControl.Enable();
        
            
        movementInput = inputControl.Gameplay.Move.ReadValue<Vector2>();
        
        switchAnimation();
        Running();
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
    }

    private void OnDisable()
    {
        inputControl.Disable();
        EventHandler.beforeSceneUnloadEvent -= onBeforeSceneUnloadEvent;
        EventHandler.afterSceneLoadedEvent -= onAfterSceneLoadEvent;
        EventHandler.moveToPosition -= onMoveToPosition;
        EventHandler.mouseClickEvent -= onMouseClickEvent;
        EventHandler.updateGameStateEvent -= onUpdateGameStateEvent;
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
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            speed =1000;
        }


        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed = 500;
        }
    }

    public void switchAnimation()
    {
        anim.SetBool("isMoving", isMoving);
        if (isMoving)
        {
            anim.SetFloat("velocityX", rb.velocity.x);
            anim.SetFloat("velocityY", rb.velocity.y);
        }
        
    }
}
