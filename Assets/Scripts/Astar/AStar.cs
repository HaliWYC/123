using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShanHai_IsolatedCity.Map;

namespace ShanHai_IsolatedCity.Astar
{
    public class AStar : Singleton<AStar>
    {
        private GridNodes gridNodes;
        private Node startNode;
        private Node targetNode;

        private int gridWidth;
        private int gridHeight;
        private int originX;
        private int originY;

        private List<Node> openNodeList; //Eight nodes surrounds the current node
        private HashSet<Node> closeNodeList; //All the selected nodes
        private bool pathFound;

        public void BuildPath(string sceneName, Vector2Int startPos, Vector2Int endPos,Stack<MovementStep> npcMovementStack)
        {
            pathFound = false;
            if (GenerateGridNodes(sceneName, startPos, endPos))
            {
                
                //Find the shortest path
                if (FindShortestPath())
                {

                    //Generate NPC paths
                    
                    UpdatePathOnMovementStepStack(sceneName, npcMovementStack);

                }
            }
        }

        private bool GenerateGridNodes(string sceneName,Vector2Int startPos,Vector2Int endPos)
        {
            if(GridMapManager.Instance.GetGridDimensions(sceneName,out Vector2Int gridDimension,out Vector2Int gridOrigin))
            {
                gridNodes = new GridNodes(gridDimension.x, gridDimension.y);
                gridWidth = gridDimension.x;
                gridHeight = gridDimension.y;
                originX = gridOrigin.x;
                originY = gridOrigin.y;

                openNodeList = new List<Node>();

                closeNodeList = new HashSet<Node>();
            }
            else
            return false;

            //The range of gridnodes starts from 0,0, so it should minues
            startNode = gridNodes.GetGridNode(startPos.x - originX, startPos.y - originY);
            targetNode = gridNodes.GetGridNode(endPos.x - originX, endPos.y - originY);

            for(int x = 0; x < gridWidth; x++)
            {
                for(int y = 0; y < gridHeight; y++)
                {
                    
                    Vector3Int tilePos = new Vector3Int(x + originX, y + originY, 0);
                    var key = tilePos.x + "x" + tilePos.y + "y" + sceneName;

                    TileDetails tile = GridMapManager.Instance.GetTileDetails(key);

                    if (tile != null)
                    {
                        Node node = gridNodes.GetGridNode(x, y);

                        if (tile.isNPCObstacle)
                            node.isObstacle = true;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Find all the node in the shortest path and add them into closetNodeList
        /// </summary>
        /// <returns></returns>
        private bool FindShortestPath()
        {
            //Add start point

            openNodeList.Add(startNode);

            while (openNodeList.Count > 0)
            {
                openNodeList.Sort();

                Node closeNode = openNodeList[0];

                openNodeList.RemoveAt(0);
                closeNodeList.Add(closeNode);

                if (closeNode == targetNode)
                {
                    pathFound = true;
                    break;
                }

                EvaluateNeighbourNodes(closeNode);
            }
            return pathFound;
            
        }

        /// <summary>
        /// Evaluate eight near node and generate matching costs
        /// </summary>
        /// <param name="currentNode"></param>
        private void EvaluateNeighbourNodes(Node currentNode)
        {
            Vector2Int currentNodePos = currentNode.gridPosition;
            Node validNeighbourNode;

            for(int x = -1; x <= 1; x++)
            {
                for(int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0)
                        continue;

                    validNeighbourNode = GetValidNeighbourNodes(currentNodePos.x+ x, currentNodePos.y+ y);

                    if (validNeighbourNode != null)
                    {
                        if (!openNodeList.Contains(validNeighbourNode))
                        {
                            validNeighbourNode.gCost = currentNode.gCost + GetDistance(currentNode, validNeighbourNode);
                            validNeighbourNode.hCost = GetDistance(validNeighbourNode, targetNode);
                            //Link to parent node
                            validNeighbourNode.parentNode = currentNode;
                            openNodeList.Add(validNeighbourNode);
                        }
                    }
                }
            }
        }

        private Node GetValidNeighbourNodes(int x, int y)
        {
            if (x >= gridWidth || y >= gridHeight || x < 0 || y < 0)
                return null;

            Node neighbourNode = gridNodes.GetGridNode(x, y);

            if (neighbourNode.isObstacle || closeNodeList.Contains(neighbourNode))
                return null;
            else
                return neighbourNode;
        }


        private int GetDistance(Node nodeA, Node nodeB)
        {
            int xDistance = Mathf.Abs(nodeA.gridPosition.x - nodeB.gridPosition.x);
            int yDistance = Mathf.Abs(nodeA.gridPosition.y - nodeB.gridPosition.y);

            if (xDistance > yDistance)
            {
                return 14 * xDistance + 10 * (xDistance - yDistance);
            }
            return 14 * yDistance + 10 * (yDistance - xDistance);
        }

        private void UpdatePathOnMovementStepStack(string sceneName, Stack<MovementStep> npcMovementStep)
        {
            Node nextNode = targetNode;

            while (nextNode != null)
            {
                MovementStep newStep = new MovementStep();
                newStep.sceneName = sceneName;
                newStep.gridCoordinate = new Vector2Int(nextNode.gridPosition.x + originX, nextNode.gridPosition.y + originY);
                
                //Push into stack
                npcMovementStep.Push(newStep);
                nextNode = nextNode.parentNode;
            }
        }
    }
}
