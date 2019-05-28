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
            currentNode.gCost = int.MaxValue;
            currentNode.hCost = GetDistance(seekerNode, targetNode);
            currentNode.parent = seekerNode;

            Node[] neighboursNodes;

            List<Node> closeNodes = new List<Node>();
            List<Node> path = new List<Node>();

            closeNodes.Add(currentNode);

            while (currentNode.position != targetNode.position)
            {
                neighboursNodes = grid.GetNeighbours(currentNode);
                for (int i = 0; i < neighboursNodes.Length; i++)
                {
                    if (IsWalkable(neighboursNodes[i].position) ||
                        SearchNode(closeNodes, neighboursNodes[i]) == null)
                    {
                        neighboursNodes[i].gCost = currentNode.gCost + GetDistance(currentNode, neighboursNodes[i]);
                        neighboursNodes[i].hCost = GetDistance(neighboursNodes[i], targetNode);
                        neighboursNodes[i].parent = currentNode;
                        closeNodes.Add(neighboursNodes[i]);
                    }
                }
                currentNode = FindingNextCurrentNode(closeNodes);
            }
            
            //currentNode = closeNodes[closeNodes.Count - 1];

            //do
            //{
            //    path.Add(currentNode);
            //    currentNode = currentNode.parent;
            //} while (currentNode != closeNodes[0]);

            //    path.Add(targetNode);
            //path.Add(seekerNode);

            return closeNodes;
        }

        /// <summary>
        /// Берёт все Node из list с самым маленьким fCost
        /// Потом ищет с самым маленьким hCost
        /// И отправляет результат
        /// </summary>
        /// <param name="list">List из которого ищем следующую Node</param>
        /// <returns>Следующий Node</returns>
        private static Node FindingNextCurrentNode(List<Node> list)
        {
            Node nextCurrentNode = null;
            if (list.Count > 0)
            {
                nextCurrentNode = list[0];
                for (int i = 0; i < list.Count; i++)
                {
                    if(nextCurrentNode.CompareTo(list[i]) < 0)
                    {
                        nextCurrentNode = list[i];
                    }
                }
            }
            return nextCurrentNode;

            //return (from node in (from node2 in list
            //                     where node2 == (from node3 in list
            //                                    orderby node3.fCost
            //                                    select node3).First()
            //                     select node2)
            //        orderby node.hCost
            //        select node).First();
        }

        /// <summary>
        /// Проверяет List на наличие элемента
        /// </summary>
        /// <param name="list">Проверяемый список</param>
        /// <param name="n">Искомый элемент</param>
        /// <returns>Присутствие/Отсутствие объекта</returns>
        private static Node SearchNode(List<Node> list, Node n)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].position == n.position)
                {
                    return list[i];
                }
            }
            return null;
            //return list.Find((x) => x == grid.NodeFromWorldPosition(n.position)) != null;
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

            return Mathf.Sqrt(Mathf.Pow(dstX, 2) + Mathf.Pow(dstY, 2) + Mathf.Pow(dstZ, 2));
        }
    }
}