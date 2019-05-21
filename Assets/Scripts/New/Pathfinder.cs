﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace New
{
    public class Pathfinder
    {
        private static New.GridController grid;

        public static List<Node> FindPath(Vector3 startPos, Vector3 targetPos)
        {
            grid = GameObject.Find("NewGridController").GetComponent<New.GridController>();

            Node startNode = grid.NodeFromWorldPosition(startPos);
            Node targetNode = grid.NodeFromWorldPosition(targetPos);

            Node currentNode = startNode;
            currentNode.gCost = 0;
            currentNode.parent = startNode;

            Node[] neighboursNodes;

            List<Node> closeNodes = new List<Node>();
            List<Node> path = new List<Node>();

            closeNodes.Add(startNode);

            while (currentNode.position != targetNode.position)
            {
                neighboursNodes = grid.GetNeighbours(startNode);
                for (int i = 0; i < neighboursNodes.Length; i++)
                {
                    if (IsWalkable(neighboursNodes[i].position)) continue;
                    neighboursNodes[i].gCost = currentNode.gCost + GetDistance(neighboursNodes[i], startNode);
                    neighboursNodes[i].hCost = GetDistance(neighboursNodes[i], targetNode);
                    neighboursNodes[i].parent = currentNode;
                    closeNodes.Add(neighboursNodes[i]);
                }
                currentNode = FindingCurrentNode(closeNodes);
            }

            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.parent;
            }

            return path;
        }

        /// <summary>
        /// Берёт все Node из list с самым маленьким fCost
        /// Потом ищет с самым маленьким hCost
        /// И отправляет результат
        /// </summary>
        /// <param name="list">List из которого ищем следующую Node</param>
        /// <returns>Следующий Node</returns>
        private static Node FindingCurrentNode(List<Node> list)
        {
            return (from node in (from node2 in list
                                 where node2 == (from node3 in list
                                                orderby node3.fCost
                                                select node3).First()
                                 select node2)
                    orderby node.hCost
                    select node).First();
        }

        private static bool IsWalkable(Vector3 position)
        {
            return !(Physics.CheckSphere(position, grid.nodeRadius, 1 << LayerMask.NameToLayer("Frame")));
        }

        private static float GetDistance(Node nodeA, Node nodeB)
        {
            int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
            int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);
            int dstZ = Mathf.Abs(nodeA.gridZ - nodeB.gridZ);

            return Mathf.Pow(dstX, 2) + Mathf.Pow(dstY, 2) + Mathf.Pow(dstZ, 2);
        }
    }
}