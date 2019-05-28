using System.Collections.Generic;
using UnityEngine;

public class Pathfinder
{
    private static GridController grid;
    private static List<Node> path;

    /// <summary>
    /// Находит путь
    /// </summary>
    /// <param name="startPos">Стартовая позиция</param>
    /// <param name="targetPos">Позиция цели</param>
    public static List<Node> FindPath(Vector3 startPos, Vector3 targetPos)
    {
        grid = GameObject.Find("GridController").GetComponent<GridController>();
        //grid.GridUpdate();
        path = new List<Node>();
        Node startNode = grid.NodeFromWorldPosition(startPos);
        Node targetNode = grid.NodeFromWorldPosition(targetPos + Vector3.up * (grid.NodeDiameter - grid.NodeInterval));

        Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while(openSet.Count > 0)
        {
            Node currentNode = openSet.RemoveFirst();
            closedSet.Add(currentNode);

            if(currentNode == targetNode)
            {
                RetracePath(startNode, targetNode);
                return path;
            }

            foreach(Node neighbour in grid.GetNeighbours(currentNode))
            {
                if(!neighbour.walkable || closedSet.Contains(neighbour))
                {
                    continue;
                }

                float newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                if(newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                }
            }
        }
        return path;
    }

    private static void RetracePath(Node startNode, Node endNode)
    {
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            if(currentNode == endNode ||
               currentNode == startNode)
            {
                path.Add(Gravity(currentNode));
            }

            if(Gravity(currentNode).position.y != Gravity(currentNode.parent).position.y)
            {
                path.Add(Gravity(currentNode));
                path.Add(Gravity(currentNode.parent));
            }

            currentNode = currentNode.parent;
        }
        path.Reverse();
    }

    private static Node Gravity(Node node)
    {
        RaycastHit hit;
        Node gravityNode = node;
        if (Physics.Raycast(node.position, -Vector3.up, out hit, Mathf.Infinity, grid.UnwalkableMask)) // Заменить на Frame
        {
            gravityNode = grid.NodeFromWorldPosition(new Vector3(node.position.x, hit.transform.position.y + 0.1f/* + seeker.localScale.y / 2*/, node.position.z));
        }
        return gravityNode;
    }

    /// <summary>
    /// Находит дистанцию между двумя Node-ами
    /// </summary>
    /// <param name="nodeA">Первая Node-а</param>
    /// <param name="nodeB">Вторая Node-а</param>
    /// <returns>Дистанция</returns>
    private static float GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);
        int dstZ = Mathf.Abs(nodeA.gridZ - nodeB.gridZ);

        return Mathf.Sqrt(Mathf.Pow(dstX, 2) + Mathf.Pow(dstY, 2) + Mathf.Pow(dstZ, 2));
    }
}
