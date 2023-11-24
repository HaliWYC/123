﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShanHai_IsolatedCity.Astar;
using UnityEngine.SceneManagement;
using System;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class NPCMovement : MonoBehaviour
{
    public ScheduleDataList_SO scheduleData;
    private SortedSet<ScheduleDetails> scheduleSet;
    private ScheduleDetails currentSchedule;

    [SerializeField] private string currentScene;
    private string targetScene;
    private Vector3Int currentGridPosition;
    private Vector3Int targetGridPosition;
    private Vector3Int nextGridPosition;
    private Vector3 nextWorldPosition;

    public string startScene { set => currentScene = value; }

    [Header("移动属性")]
    public float normalSpeed = 2f;
    private float minSpeed = 1;
    private float maxSpeed = 3;
    private Vector2 dir;
    public bool isMoving;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D coll;
    private Animator anim;

    private Grid grid;
    private bool isInitailised;
    private bool npcMove;
    private bool sceneLoaded;
    public bool interactable;

    //Counting
    private float animationBreakTime; 
    private bool canPlayStopAnimaiton;
    private AnimationClip stopAnimationClip;
    public AnimationClip blankAnimationClip;
    private AnimatorOverrideController animOverride;

    private TimeSpan gameTime => TimeManager.Instance.gameTime;

    private Stack<MovementStep> movementSteps;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        movementSteps = new Stack<MovementStep>();

        animOverride = new AnimatorOverrideController(anim.runtimeAnimatorController);
        anim.runtimeAnimatorController = animOverride;
        scheduleSet = new SortedSet<ScheduleDetails>();

        foreach(var schedule in scheduleData.scheduleList)
        {
            //Debug.Log(schedule.targetScene);
            scheduleSet.Add(schedule);
        }
    }

    private void OnEnable()
    {
        EventHandler.afterSceneLoadedEvent += onAfterSceneLoadedEvent;
        EventHandler.beforeSceneUnloadEvent += onBeforeSceneUnloadEvent;
        EventHandler.gameMinuteEvent += onGameMinuteEvent;
    }

    

    private void OnDisable()
    {
        EventHandler.afterSceneLoadedEvent -= onAfterSceneLoadedEvent;
        EventHandler.beforeSceneUnloadEvent -= onBeforeSceneUnloadEvent;
        EventHandler.gameMinuteEvent += onGameMinuteEvent;
    }

    private void Update()
    {
        if (sceneLoaded)
            switchAnimation();

        //Counting
        animationBreakTime -= Time.deltaTime;
        canPlayStopAnimaiton = animationBreakTime <= 0;
    }

    private void FixedUpdate()
    {
        if(sceneLoaded)
        movement();
    }

    private void onBeforeSceneUnloadEvent()
    {
        sceneLoaded = false;
    } 
    private void onAfterSceneLoadedEvent()
    {
        grid = FindObjectOfType<Grid>();
        checkVisiable();
        if (!isInitailised)
        {
            initNPC();
            //Debug.Log("Yes");
            isInitailised = true;
        }

        sceneLoaded = true;

    }

    private void onGameMinuteEvent(int minute, int hour,int day,  Seasons season)
    {
        int time = (hour * 100) + minute;

        ScheduleDetails matchSchedule = null;
        foreach(var schedule in scheduleSet)
        {
            if(schedule.Time == time)
            {
                if (schedule.day != day&&schedule.day!=0)
                    continue;
                if (schedule.season != season)
                    continue;
                matchSchedule = schedule; 
            }
            else if (schedule.Time > time)
            {
                break;
            }
            
        }
        
        if (matchSchedule != null)
        {
            Debug.Log(matchSchedule);
            buildSchedulePath(matchSchedule);
        }

    }

    private void checkVisiable()
    {
        
        if(currentScene == SceneManager.GetActiveScene().name)
        {
            setActiveInScene();
        }
        else
        {
            setInactiveInScene();
        }
    }

    private void initNPC()
    {
        targetScene = currentScene;

        //Keep in the center of the tile
        currentGridPosition = grid.WorldToCell(transform.position);
        transform.position = new Vector3(currentGridPosition.x + Settings.gridCellSize / 2f, currentGridPosition.y + Settings.gridCellSize / 2f, 0);
        targetGridPosition = currentGridPosition;
        
    }

    private void movement()
    {
        if (!npcMove)
        {
            //Debug.Log(movementSteps);
            if (movementSteps.Count > 0)
            {
                MovementStep step = movementSteps.Pop();

                currentScene = step.sceneName;

                checkVisiable();

                nextGridPosition = (Vector3Int)step.gridCoordinate;
                //Debug.Log(nextGridPosition);

                TimeSpan stepTime = new TimeSpan(step.hour, step.minute, step.second);

                moveToGridPosition(nextGridPosition, stepTime);
            }
            else if (!isMoving && canPlayStopAnimaiton)
            {
                StartCoroutine(setStopAnimation());
            }
        }
        

    }

    private void moveToGridPosition(Vector3Int gridPos, TimeSpan stepTime)
    {
        StartCoroutine(moveRoutine(gridPos, stepTime));
    }

    private IEnumerator moveRoutine(Vector3Int gridPos, TimeSpan stepTime)
    {
        npcMove = true;
        nextWorldPosition = getWorldPosition(gridPos);
        //Remains time
        if (stepTime > gameTime)
        {
            float timeTomove = (float)(stepTime.TotalSeconds - gameTime.TotalSeconds);
            float distance = Vector3.Distance(transform.position, nextWorldPosition);
            float speed = Mathf.Max(minSpeed, (distance / timeTomove / Settings.secondThrehold));

            if (speed <= maxSpeed)
            {
                while (Vector3.Distance(transform.position, nextWorldPosition)>Settings.pixelSize)
                {
                    dir = (nextWorldPosition - transform.position).normalized;

                    Vector2 posOffset = new Vector2(dir.x * speed * Time.fixedDeltaTime, dir.y * speed * Time.fixedDeltaTime);
                    rb.MovePosition(rb.position + posOffset);
                    yield return new WaitForFixedUpdate();
                }
            }

        }
        rb.position = nextWorldPosition;
        currentGridPosition = gridPos;
        nextGridPosition = currentGridPosition;
        npcMove = false;
    }


    /// <summary>
    /// Build the path accroding to the schedule
    /// </summary>
    /// <param name="schedule"></param>
    public void buildSchedulePath(ScheduleDetails schedule)
    {
        movementSteps.Clear();
        currentSchedule = schedule;
        targetGridPosition = (Vector3Int)schedule.targetGridposition;
        stopAnimationClip = schedule.clipAtStop;
        this.interactable = schedule.interactable; 

        if (schedule.targetScene == currentScene)
        {
            AStar.Instance.buildPath(schedule.targetScene, (Vector2Int)currentGridPosition, schedule.targetGridposition, movementSteps);
        }
        else if (schedule.targetScene != currentScene)
        {
            SceneRoute sceneRoute = NPCManager.Instance.getSceneroute(currentScene, schedule.targetScene);

            if (sceneRoute != null)
            {
                for(int i = 0; i < sceneRoute.scenePathList.Count; i++)
                {
                    Vector2Int fromPos, goToPos;
                    ScenePath path = sceneRoute.scenePathList[i];

                    if (path.fromGridCell.x >= Settings.maxGridSize)
                    {
                        fromPos = (Vector2Int)currentGridPosition;
                    }
                    else
                    {
                        fromPos = path.fromGridCell;
                    }

                    if (path.goToGridCell.x >= Settings.maxGridSize)
                    {
                        goToPos = schedule.targetGridposition;
                    }
                    else
                    {
                        goToPos = path.goToGridCell;
                    }

                    //Debug.Log(fromPos + goToPos);

                    AStar.Instance.buildPath(path.sceneName, fromPos, goToPos, movementSteps);
                }
            }
        }


        if (movementSteps.Count > 1)
        {
            updatTimeOnPath();
        }
    }

    private void updatTimeOnPath()
    {
        MovementStep previousStep = null;
        TimeSpan currentGameTime = gameTime;

        foreach(MovementStep step in movementSteps)
        {
            if(previousStep == null)
            {
                previousStep = step;

            }

            step.hour = currentGameTime.Hours;
            step.minute = currentGameTime.Minutes;
            step.second = currentGameTime.Seconds;

            TimeSpan gridMovementStepTime;
            if(moveInDiagonal(step,previousStep))
                gridMovementStepTime = new TimeSpan(0, 0, (int)(Settings.gridCellDiagonalSize / normalSpeed / Settings.secondThrehold));

            else
                gridMovementStepTime = new TimeSpan(0, 0, (int)(Settings.gridCellSize / normalSpeed / Settings.secondThrehold));

            currentGameTime = currentGameTime.Add(gridMovementStepTime);
            //Looping next step
            previousStep = step;
        }
    }

    /// <summary>
    /// Check whether is moving diagonally or not
    /// </summary>
    /// <param name="currentStep"></param>
    /// <param name="previousStep"></param>
    /// <returns></returns>
    private bool moveInDiagonal(MovementStep currentStep,MovementStep previousStep)
    {
        return (currentStep.gridCoordinate.x != previousStep.gridCoordinate.x) && (currentStep.gridCoordinate.y != previousStep.gridCoordinate.y);
    }

    private Vector3 getWorldPosition(Vector3Int gridPos)
    {
        Vector3 worldPos = grid.CellToWorld(gridPos);
        return new Vector3(worldPos.x + Settings.gridCellSize / 2f, worldPos.y + Settings.gridCellSize / 2);
    }


    private void switchAnimation()
    {
        isMoving = transform.position != getWorldPosition(targetGridPosition);
        anim.SetBool("isMoving", isMoving);
        if (isMoving)
        {
            anim.SetBool("Exit", true);
            anim.SetFloat("DirX", dir.x);
            anim.SetFloat("DirY", dir.y);
        }
        else
        {
            anim.SetBool("Exit", false);
        }

    }


    private IEnumerator setStopAnimation()
    {
        //Face camera
        anim.SetFloat("DirX", 0);
        anim.SetFloat("DirY", -1);

        animationBreakTime = Settings.animationBreakTime;

        if (stopAnimationClip != null)
        {
            animOverride[blankAnimationClip] = stopAnimationClip;
            anim.SetBool("EventAnimation", true);
            yield return null;
            anim.SetBool("EventAnimation", false);
        }
        else
        {
            animOverride[stopAnimationClip] = blankAnimationClip;
            anim.SetBool("EventAnimation", false);
        }
    }

    #region 设置NPC的显示
    private void setActiveInScene()
    {
        spriteRenderer.enabled = true;
        coll.enabled = true;
        transform.GetChild(0).gameObject.SetActive(true);
    }

    private void setInactiveInScene()
    {
        spriteRenderer.enabled = false;
        coll.enabled = false;
        transform.GetChild(0).gameObject.SetActive(false);
    }
    #endregion
}
