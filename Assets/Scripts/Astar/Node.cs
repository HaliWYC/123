using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ShanHai_IsolatedCity.Astar
{
    public class Node : IComparable<Node>
    {
        public Vector2Int gridPosition;
        public int gCost = 0;//Distance from start point
        public int hCost = 0;//Distance from target point
        public int FCost => gCost + hCost;//Current grid value
        public bool isObstacle = false;//Whether current grid is a obstacle or not
        public Node parentNode;

        public Node(Vector2Int pos)
        {
            gridPosition = pos;
            parentNode = null;
        }

        public int CompareTo(Node other)
        {
            int result = FCost.CompareTo(other.FCost);
            if (result == 0)
            {
                result = hCost.CompareTo(other.hCost);
            }
            return result;
        }
    }

}
