using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using ShanHai_IsolatedCity.Map;

public class CursorManager : Singleton<CursorManager>
{
    public Sprite normal, attack, item, interact;

    private Sprite currenSprite;//Current cursor sprite

    private Image cursorImage;

    private RectTransform cursorCanvas;



    //Cursor Detect
    private Camera mainCamera;
    public Grid currentGrid;
    private Vector3 mouseWorldPos;
    private Vector3Int mouseGridPos;
    private bool cursorEnable;
    public bool cursorPositionValid;
    private ItemDetails currentItem;
    private Transform playertransform => FindObjectOfType<Player>().transform;


    private void OnEnable()
    {
        
        EventHandler.itemSelectedEvent += onItemSelectedEvent;
        EventHandler.beforeSceneUnloadEvent += onBeforeSceneUnloadEvent;
        EventHandler.afterSceneLoadedEvent += onAfterSceneLoadedEvent;


    }

    private void onBeforeSceneUnloadEvent()
    {
        cursorEnable = false;
    }

    private void onAfterSceneLoadedEvent()
    {
        currentGrid = FindObjectOfType<Grid>();
        
    }

    private void OnDisable()
    {
        EventHandler.itemSelectedEvent -= onItemSelectedEvent;
        EventHandler.beforeSceneUnloadEvent -= onBeforeSceneUnloadEvent;
        EventHandler.afterSceneLoadedEvent -= onAfterSceneLoadedEvent;
    }

    private void onItemSelectedEvent(ItemDetails itemDetails, bool isSelected)
    {
        
        if (!isSelected)
        {
            currentItem = null;
            cursorEnable = false;
            currenSprite = normal;
        }
        else
        {
            currentItem = itemDetails;
            //WorkFlow: Add all the image related to items
            currenSprite = itemDetails.itemType switch
            {
                ItemType.商品 => item,
                ItemType.武器 => attack,
                _ => normal

            };
            cursorEnable = true;
            //Debug.Log(currenSprite);
        }
        
    }

    private void Start()
    {
        cursorCanvas = GameObject.FindGameObjectWithTag("CursorCanvas").GetComponent<RectTransform>();
        cursorImage = cursorCanvas.GetChild(0).GetComponent<Image>();
        currenSprite = normal;
        mainCamera = Camera.main;

        setCursorImage(normal);
    }


    private void Update()
    {
        if (cursorCanvas == null) return;

        //Debug.Log();
        cursorImage.transform.position = Input.mousePosition;
        
        if (!interactWithUI() && cursorEnable)
        {
            setCursorImage(currenSprite);
            checkCursorValid();
            checkPlayerInput();
        }
        else
        {
            setCursorImage(normal);
        }

    }

    private void checkPlayerInput()
    {
        if(Input.GetMouseButtonDown(0) && cursorPositionValid)
        {
            //Execute the method
            EventHandler.callMouseClickEvent(mouseWorldPos, currentItem);
        }
    }

    #region SetCursorStyle
    private void setCursorImage(Sprite sprite)
    {
        cursorImage.sprite = sprite;
        cursorImage.color = new Color(1, 1, 1, 1);
    }
    /// <summary>
    /// Set cursor valid
    /// </summary>
    private void setCursorValid()
    {
        cursorPositionValid = true;
        cursorImage.color = new Color(1, 1, 1, 1);
    }
    /// <summary>
    /// Set cursor invalid
    /// </summary>
    private void setCursorInvalid()
    {
        cursorPositionValid = false;
        cursorImage.color = new Color(1, 0, 0, 0.5f);
    }

    #endregion
    private void checkCursorValid()
    {
        
        mouseWorldPos = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -mainCamera.transform.position.z));
        mouseGridPos = currentGrid.WorldToCell(mouseWorldPos);

        var playerGridPos = currentGrid.WorldToCell(playertransform.position);

        //Check whether in the use radius or not
        if (Mathf.Abs(mouseGridPos.x - playerGridPos.x) > currentItem.useRadius || Mathf.Abs(mouseGridPos.y - playerGridPos.y) > currentItem.useRadius)
        {
            
            setCursorInvalid();
            return;
        }
        //Debug.Log(Mathf.Abs(mouseGridPos.x - playerGridPos.x) > currentItem.useRadius);
        //Debug.Log(Mathf.Abs(mouseGridPos.y - playerGridPos.y) > currentItem.useRadius);
        //Debug.Log(cursorPositionValid);
        TileDetails currentTile = GridMapManager.Instance.getTileDetailsOnMousePosition(mouseGridPos);
        TileDetails playerTile = GridMapManager.Instance.getTileDetailsOnMousePosition(playerGridPos);

        if (currentTile != null)
        {
            switch (currentItem.itemType)
            {
                case ItemType.商品:
                    if (currentTile.canDropItem&&currentItem.canDrop) setCursorValid(); else setCursorInvalid();
                    break;
                case ItemType.武器:
                    if (playerTile!=null && playerTile.meleeOnly) setCursorValid(); else setCursorInvalid();
                    break;
            }
        }
        else
        {
            setCursorInvalid();
            
        }
    }

    /// <summary>
    /// Whether interact with UI or not
    /// </summary>
    /// <returns></returns>
    private bool interactWithUI()
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            return true;
        }

        return false;

    }
}
