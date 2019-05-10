using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public Transform seeker;
    public Transform target;

    Grid grid;

    private void Awake()
    {
        grid = GetComponent<Grid>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FindPath(seeker.position, target.position);
        }
    }

    /// <summary>
    /// Находит путь
    /// </summary>
    /// <param name="startPos">Стартовая позиция</param>
    /// <param name="targetPos">Позиция цели</param>
    private void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();
        Node startNode = grid.NodeFromWorldPosition(startPos);
        Node targetNode = grid.NodeFromWorldPosition(targetPos);

        Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while(openSet.Count > 0)
        {
            Node currentNode = openSet.RemoveFirst();
            closedSet.Add(currentNode);

            if(currentNode == targetNode)
            {
                sw.Stop();
                print($"Path found: {sw.ElapsedMilliseconds} ms");
                RetracePath(startNode, targetNode);
                return;
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
    }

    private void RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;
        RaycastHit hit;

        while (currentNode != startNode)
        {
            // Применяем гравитацию
            Node gravityNode = currentNode;
            if (Physics.Raycast(currentNode.worldPosition, -Vector3.up, out hit, Mathf.Infinity, grid.UnwalkableMask))
            {
                gravityNode = grid.NodeFromWorldPosition(new Vector3(currentNode.worldPosition.x, hit.transform.position.y + grid.NodeRadius + seeker.localScale.y / 2, currentNode.worldPosition.z));
            }
            ///////////////////////
            path.Add(gravityNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();
        grid.path = path;
    }

    /// <summary>
    /// Находит дистанцию между двумя Node-ами
    /// </summary>
    /// <param name="nodeA">Первая Node-а</param>
    /// <param name="nodeB">Вторая Node-а</param>
    /// <returns>Дистанция</returns>
    private float GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);
        int dstZ = Mathf.Abs(nodeA.gridZ - nodeB.gridZ);

        return Mathf.Sqrt(Mathf.Pow(dstX, 2) + Mathf.Pow(dstY, 2) + Mathf.Pow(dstZ, 2));
    }
}
