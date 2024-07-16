using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ShanHai_IsolatedCity.Map
{
    public class GridMapManager : Singleton<GridMapManager>
    {
        [Header("地图信息")]
        public List<MapData_SO> mapDataList;

        //SceneName+postion and matched Tilemap information
        private Dictionary<string, TileDetails> tileDetailsDict = new Dictionary<string, TileDetails>();

        private Grid currentGrid;


        private void Start()
        {
            foreach (var mapData in mapDataList)
            {
                initTileDetailsDict(mapData);
            }
        }



        /// <summary>
        /// Generate Dictionary accroding to the Map information
        /// </summary>
        /// <param name="mapData">Map information</param>
        private void initTileDetailsDict(MapData_SO mapData)
        {
            foreach (TileProperty tileProperty in mapData.tileProperties)
            {
                TileDetails tileDetails = new TileDetails
                {
                    gridX = tileProperty.tileCoordinate.x,
                    gridY = tileProperty.tileCoordinate.y
                };

                //Key of dictionary
                string key = tileDetails.gridX + "x" + tileDetails.gridY + "y" + mapData.sceneName;

                if (GetTileDetails(key) != null)
                {
                    tileDetails = GetTileDetails(key);
                }

                switch (tileProperty.gridType)
                {
                    case GridType.可投掷区:
                        tileDetails.canDropItem = tileProperty.boolTypeValue;
                        break;
                    case GridType.近战区:
                        tileDetails.meleeOnly = tileProperty.boolTypeValue;
                        break;
                    case GridType.远程区:
                        tileDetails.rangedOnly = tileProperty.boolTypeValue;
                        break;
                    case GridType.NPC障碍:
                        tileDetails.isNPCObstacle = tileProperty.boolTypeValue;
                        break;
                }

                if (GetTileDetails(key) != null)
                    tileDetailsDict[key] = tileDetails;
                else
                    tileDetailsDict.Add(key, tileDetails);
            }
        }

        private void OnEnable()
        {
            EventHandler.ExecuteActionAfterAnimation += OnExecuteActionAfterAnimation;
            EventHandler.AfterSceneLoadedEvent += OnAfterSceneLoadedEvent;
        }

        private void OnDisable()
        {
            EventHandler.ExecuteActionAfterAnimation -= OnExecuteActionAfterAnimation;
            EventHandler.AfterSceneLoadedEvent -= OnAfterSceneLoadedEvent;
        }

        private void OnAfterSceneLoadedEvent()
        {
            currentGrid = FindFirstObjectByType<Grid>();
        }

        private void OnExecuteActionAfterAnimation(Vector3 mouseWorldPos, ItemDetails itemDetails)
        {
            var mouseGridPos = currentGrid.WorldToCell(mouseWorldPos);
            var currentTile = GetTileDetailsOnMousePosition(mouseGridPos);

            if (currentTile != null)
            {
                switch (itemDetails.itemType)
                {
                    case ItemType.Other:
                        EventHandler.CallDropItemEvent(itemDetails.itemID, mouseWorldPos);
                        break;
                }
            }

        }

        /// <summary>
        /// Return Tilemap information accroding to the key
        /// </summary>
        /// <param name="key">x+y+Name of Map</param>
        /// <returns></returns>
        public TileDetails GetTileDetails(string key)
        {
            if (tileDetailsDict.ContainsKey(key))
            {
                return tileDetailsDict[key];
            }
            return null;
        }

        /// <summary>
        /// Return tilemap information accroding to the mouse position
        /// </summary>
        /// <param name="mouseGridPos">mouse postion</param>
        /// <returns></returns>
        public TileDetails GetTileDetailsOnMousePosition(Vector3Int mouseGridPos)
        {
            string key = mouseGridPos.x + "x" + mouseGridPos.y + "y" + SceneManager.GetActiveScene().name;
            return GetTileDetails(key);
        }

        /// <summary>
        /// Return range and orgin accroding scene name
        /// </summary>
        /// <param name="sceneName">Scene Name</param>
        /// <param name="gridDimension">Grid Dimension</param>
        /// <param name="gridOrigin">Grid Origin</param>
        /// <returns>Whether has current scene information or not </returns>
        public bool GetGridDimensions(string sceneName,out Vector2Int gridDimension,out Vector2Int gridOrigin)
        {
            gridDimension = Vector2Int.zero;
            gridOrigin = Vector2Int.zero;

            foreach(var mapData in mapDataList)
            {
                if(mapData.sceneName == sceneName)
                {
                    gridDimension.x = mapData.gridWidth;
                    gridDimension.y = mapData.gridHeight;

                    gridOrigin.x = mapData.originX;
                    gridOrigin.y = mapData.orginY;
                    return true;
                }
            }
            return false;
        }

        public TileDetails GetTileDetailsAtTargetPosition(Vector3 targetPos)
        {
            Vector3Int gridPos = new Vector3Int((int)targetPos.x, (int)targetPos.y, 0);
            string key = gridPos.x + "x" + gridPos.y + "y" + SceneManager.GetActiveScene().name;
            return GetTileDetails(key) != null ? GetTileDetails(key) : null;
        }
    }
}
