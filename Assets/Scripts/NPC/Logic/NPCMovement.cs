using System.Collections;
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
    private TimeSpan gameTime => TimeManager.Instance.gameTime;

    private Stack<MovementStep> movementSteps;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        EventHandler.afterSceneLoadedEvent += onAfterSceneLoadedEvent;
    }

    private void OnDisable()
    {
        EventHandler.afterSceneLoadedEvent -= onAfterSceneLoadedEvent;
    }

    private void onAfterSceneLoadedEvent()
    {
        grid = FindObjectOfType<Grid>();
        checkVisiable();
        if (!isInitailised)
        {
            initNPC();
            isInitailised = true;
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

    public void buildSchedulePath(ScheduleDetails schedule)
    {
        movementSteps.Clear();
        currentSchedule = schedule;

        if (schedule.targetScene == currentScene)
        {
            AStar.Instance.buildPath(schedule.targetScene, (Vector2Int)currentGridPosition, schedule.targetGridposition, movementSteps);
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


    #region 设置NPC的显示
    private void setActiveInScene()
    {
        spriteRenderer.enabled = true;
        coll.enabled = true;
        //TODO: Close shadow
        //transform.GetChild(0).gameObject.SetActive(true);
    }

    private void setInactiveInScene()
    {
        spriteRenderer.enabled = false;
        coll.enabled = false;
        //TODO: Close shadow
        //transform.GetChild(0).gameObject.SetActive(false);
    }
    #endregion
}
