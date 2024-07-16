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
    private Transform playertransform => FindFirstObjectByType<Player>().transform;


    private void OnEnable()
    {
        
        EventHandler.ItemSelectedEvent += OnItemSelectedEvent;
        EventHandler.BeforeSceneUnloadEvent += OnBeforeSceneUnloadEvent;
        EventHandler.AfterSceneLoadedEvent += OnAfterSceneLoadedEvent;


    }

    private void OnBeforeSceneUnloadEvent()
    {
        cursorEnable = false;
    }

    private void OnAfterSceneLoadedEvent()
    {
        currentGrid = FindFirstObjectByType<Grid>();
        
    }

    private void OnDisable()
    {
        EventHandler.ItemSelectedEvent -= OnItemSelectedEvent;
        EventHandler.BeforeSceneUnloadEvent -= OnBeforeSceneUnloadEvent;
        EventHandler.AfterSceneLoadedEvent -= OnAfterSceneLoadedEvent;
    }

    private void OnItemSelectedEvent(ItemDetails itemDetails, bool isSelected)
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
                ItemType.Other => item,
                ItemType.Equip => attack,
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

        SetCursorImage(normal);
    }


    private void Update()
    {
        if (cursorCanvas == null) return;

        //Debug.Log();
        cursorImage.transform.position = Input.mousePosition;
        
        if (!InteractWithUI() && cursorEnable)
        {
            SetCursorImage(currenSprite);
            CheckCursorValid();
            CheckPlayerInput();
        }
        else
        {
            SetCursorImage(normal);
        }

    }

    private void CheckPlayerInput()
    {
        if(Input.GetMouseButtonDown(0) && cursorPositionValid)
        {
            //Execute the method
            EventHandler.CallMouseClickEvent(mouseWorldPos, currentItem);
        }
    }

    #region SetCursorStyle
    private void SetCursorImage(Sprite sprite)
    {
        cursorImage.sprite = sprite;
        cursorImage.color = new Color(1, 1, 1, 1);
    }
    /// <summary>
    /// Set cursor valid
    /// </summary>
    private void SetCursorValid()
    {
        cursorPositionValid = true;
        cursorImage.color = new Color(1, 1, 1, 1);
    }
    /// <summary>
    /// Set cursor invalid
    /// </summary>
    private void SetCursorInvalid()
    {
        cursorPositionValid = false;
        cursorImage.color = new Color(1, 0, 0, 0.5f);
    }

    #endregion
    private void CheckCursorValid()
    {
        
        mouseWorldPos = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -mainCamera.transform.position.z));
        mouseGridPos = currentGrid.WorldToCell(mouseWorldPos);

        var playerGridPos = currentGrid.WorldToCell(playertransform.position);

        //Check whether in the use radius or not
        if (Mathf.Abs(mouseGridPos.x - playerGridPos.x) > currentItem.useRadius || Mathf.Abs(mouseGridPos.y - playerGridPos.y) > currentItem.useRadius)
        {
            
            SetCursorInvalid();
            return;
        }
        //Debug.Log(Mathf.Abs(mouseGridPos.x - playerGridPos.x) > currentItem.useRadius);
        //Debug.Log(Mathf.Abs(mouseGridPos.y - playerGridPos.y) > currentItem.useRadius);
        //Debug.Log(cursorPositionValid);
        TileDetails currentTile = GridMapManager.Instance.GetTileDetailsOnMousePosition(mouseGridPos);
        TileDetails playerTile = GridMapManager.Instance.GetTileDetailsOnMousePosition(playerGridPos);

        if (currentTile != null)
        {
            switch (currentItem.itemType)
            {
                case ItemType.Other:
                    if (currentTile.canDropItem&&currentItem.canDrop) SetCursorValid(); else SetCursorInvalid();
                    break;
                case ItemType.Equip:
                    if (playerTile!=null && playerTile.meleeOnly) SetCursorValid(); else SetCursorInvalid();
                    break;
            }
        }
        else
        {
            SetCursorInvalid();
            
        }
    }

    /// <summary>
    /// Whether interact with UI or not
    /// </summary>
    /// <returns></returns>
    private bool InteractWithUI()
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            return true;
        }

        return false;

    }
}
