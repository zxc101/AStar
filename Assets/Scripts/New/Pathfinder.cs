using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace New
{
    public class Pathfinder
    {
        private static GridController grid;

        public static List<Node> FindPath(Vector3 startPos, Vector3 targetPos)
        {
            grid = GameObject.Find("NewGridController").GetComponent<GridController>();

            Node seekerNode = grid.NodeFromWorldPosition(startPos);
            Node targetNode = grid.NodeFromWorldPosition(targetPos);

            Node currentNode = seekerNode;
            currentNode.gCost = 0;
            currentNode.parent = seekerNode;

            Node[] neighboursNodes;

            List<Node> closeNodes = new List<Node>();
            List<Node> path = new List<Node>();

            closeNodes.Add(seekerNode);

            while (currentNode.position != targetNode.position)
            {
                neighboursNodes = grid.GetNeighbours(seekerNode);
                for (int i = 0; i < neighboursNodes.Length; i++)
                {
                    if (IsWalkable(neighboursNodes[i].position) ||
                        !IsHaveNode(closeNodes, neighboursNodes[i]))
                    {
                        neighboursNodes[i].gCost = currentNode.gCost + GetDistance(neighboursNodes[i], seekerNode);
                        neighboursNodes[i].hCost = GetDistance(neighboursNodes[i], targetNode);
                        neighboursNodes[i].parent = currentNode;
                        closeNodes.Add(neighboursNodes[i]);
                    }
                }
                currentNode = FindingCurrentNode(closeNodes);
            }

            do {
                path.Add(currentNode);
                currentNode = currentNode.parent;
            } while (currentNode != seekerNode);

            //    path.Add(targetNode);
            //path.Add(seekerNode);

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

        /// <summary>
        /// Проверяет List на наличие элемента
        /// </summary>
        /// <param name="list">Проверяемый список</param>
        /// <param name="n">Искомый элемент</param>
        /// <returns>Присутствие/Отсутствие объекта</returns>
        private static bool IsHaveNode(List<Node> list, Node n)
        {
            return list.Find((x) => x == grid.NodeFromWorldPosition(n.position)) != null;
        }

        private static bool IsWalkable(Vector3 position)
        {
            return !(Physics.CheckSphere(position, grid.nodeRadius, 1 << LayerMask.NameToLayer("Frame")));
        }

        private static float GetDistance(Node nodeA, Node nodeB)
        {
            float dstX = Mathf.Abs(nodeA.position.x - nodeB.position.x);
            float dstY = Mathf.Abs(nodeA.position.y - nodeB.position.y);
            float dstZ = Mathf.Abs(nodeA.position.z - nodeB.position.z);

            return Mathf.Pow(dstX, 2) + Mathf.Pow(dstY, 2) + Mathf.Pow(dstZ, 2);
        }
    }
}