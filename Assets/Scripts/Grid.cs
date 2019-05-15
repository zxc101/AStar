using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    private const int COUNT_NODE = 80;

    //public bool onlyDisplayPathGizmos;
    //public List<Node> path;

    private float nodeRadius;
    private float nodeDiameter;
    private float nodeInterval;
    private Vector3 gridWorldSize;
    private int gridSizeX, gridSizeY, gridSizeZ;
    private Node[,,] grid;
    private GameObject[] pets;

    public int MaxSize { get => gridSizeX * gridSizeY * gridSizeZ; }
    public float NodeDiameter { get => nodeDiameter; }
    public float NodeInterval { get => nodeInterval; }
    public LayerMask UnwalkableMask { get => 1 << LayerMask.NameToLayer("Frame"); }

    private void OnValidate()
    {
        //UpdateScaleAndPosition();
    }

    private void UpdateScaleAndPosition()
    {
        pets = GameObject.FindGameObjectsWithTag("Pet");
        Vector3 allPetsPosition = Vector3.zero;
        float[] petsPositionX = new float[pets.Length];
        float[] petsPositionY = new float[pets.Length];
        float[] petsPositionZ = new float[pets.Length];
        for (int i = 0; i < pets.Length; i++)
        {
            petsPositionX[i] = pets[i].transform.position.x;
            petsPositionY[i] = pets[i].transform.position.y;
            petsPositionZ[i] = pets[i].transform.position.z;
            allPetsPosition += pets[i].transform.position;
        }
        System.Array.Sort(petsPositionX);
        System.Array.Sort(petsPositionY);
        System.Array.Sort(petsPositionZ);
        gridWorldSize.x = (petsPositionX[petsPositionX.Length - 1] + 15) - (petsPositionX[0] - 15);
        gridWorldSize.y = (petsPositionY[petsPositionY.Length - 1] + 5) - (petsPositionY[0] - 5);
        gridWorldSize.z = (petsPositionZ[petsPositionZ.Length - 1] + 15) - (petsPositionZ[0] - 15);
        transform.position = allPetsPosition / pets.Length;

        if (gridWorldSize.x <= 0)
        {
            gridWorldSize.x = 15;
        }
        if (gridWorldSize.y <= 0)
        {
            gridWorldSize.y = 5;
        }
        if (gridWorldSize.z <= 0)
        {
            gridWorldSize.z = 15;
        }
    }

    private void Awake()
    {
        //nodeRadius = Mathf.Pow((gridWorldSize.x * gridWorldSize.y/10 * gridWorldSize.z) /
        //                        Mathf.Pow(COUNT_NODE, 3), 1f / 3);
        //nodeDiameter = nodeRadius * 2;
        //nodeInterval = nodeRadius / 10;
        //GridUpdate();
    }

    /// <summary>
    /// Обновляем данные в Grid
    /// </summary>
    public void GridUpdate()
    {
        UpdateScaleAndPosition();

        nodeRadius = Mathf.Pow((gridWorldSize.x * gridWorldSize.y / 10 * gridWorldSize.z) /
                                Mathf.Pow(COUNT_NODE, 3), 1f / 3);
        nodeDiameter = nodeRadius * 2;
        nodeInterval = nodeRadius / 10;

        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        gridSizeZ = Mathf.RoundToInt(gridWorldSize.z / nodeDiameter);

        grid = new Node[gridSizeX, gridSizeY, gridSizeZ];

        ///// заполнение массива grid /////

        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2 - Vector3.forward * gridWorldSize.z / 2;
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                for (int z = 0; z < gridSizeZ; z++)
                {
                    Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.up * (y * nodeDiameter + nodeRadius + nodeInterval) + Vector3.forward * (z * nodeDiameter + nodeRadius);
                    bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, UnwalkableMask)); // Заменить на Frame
                    grid[x, y, z] = new Node(walkable, worldPoint, x, y, z);
                }
            }
        }
    }

    private void GridArrayUpdate()
    {

    }

    /// <summary>
    /// Возвращает List из соседних клеток
    /// </summary>
    /// <param name="node">Проверяемая клетка</param>
    /// <returns>Клетки по соседству</returns>
    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for(int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                for (int z = -1; z <= 1; z++)
                {
                    if (x == 0 && y == 0 && z == 0)
                    {
                        continue;
                    }

                    int checkX = node.gridX + x;
                    int checkY = node.gridY + y;
                    int checkZ = node.gridZ + z;

                    if (checkX >= 0 && checkX < gridSizeX &&
                        checkY >= 0 && checkY < gridSizeY &&
                        checkZ >= 0 && checkZ < gridSizeZ)
                    {
                        neighbours.Add(grid[checkX, checkY, checkZ]);
                    }
                }
            }
        }

        return neighbours;
    }

    /// <summary>
    /// Находит Node соответствующий заданной позиции
    /// </summary>
    /// <param name="worldPosition">Позиция в мировых координатах</param>
    /// <returns>Node соответствующий заданной позиции</returns>
    public Node NodeFromWorldPosition(Vector3 worldPosition)
    {
        worldPosition -= transform.position;
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.y + gridWorldSize.y / 2) / gridWorldSize.y;
        float percentZ = (worldPosition.z + gridWorldSize.z / 2) / gridWorldSize.z;
        
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);
        percentZ = Mathf.Clamp01(percentZ);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        int z = Mathf.RoundToInt((gridSizeZ - 1) * percentZ);

        return grid[x, y, z];
    }

    private void OnDrawGizmos()
    {
        // Отрисовывае куб в котором будет происходить всё действие
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, gridWorldSize.z));
        //if (onlyDisplayPathGizmos)
        //{
            // Показывает только клетки пути
            //if (path != null)
            //{
                //foreach(Node n in grid)
                //{
                //    if(n == NodeFromWorldPosition(seeker.position))
                //    {
                //        Gizmos.color = Color.cyan;
                //        Gizmos.DrawCube(n.worldPosition, Vector3.one * (NODE_DIAMETER - NODE_INTERVAL));
                //    }
                //}
            //    foreach(Node n in path)
            //    {
            //        Gizmos.color = new Color(255, 247, 157);
            //        Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - nodeInterval));
            //    }
            //}
        //}
        //else
        //{
        //    if (grid != null)
        //    {
        //        foreach (Node n in grid)
        //        {
        //            // Если по клетке можно пройти то отображаем как пустую иначе как не годную для ходьбы
        //            Gizmos.color = (n.walkable)? Color.clear : Color.red;

        //            // Если путь найден
        //            if (path != null)
        //            {
        //                // и ячейка соответствует ячейки пути
        //                if (path.Contains(n))
        //                    // Меняем цвет клетки
        //                    Gizmos.color = new Color(255, 247, 157);
        //            }
        //            // Рисуем
        //            Gizmos.DrawCube(n.worldPosition, Vector3.one* (nodeDiameter - nodeInterval));
        //        }
        //    }
        //}
    }
}
