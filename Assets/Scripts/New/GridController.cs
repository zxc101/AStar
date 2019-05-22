using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace New
{
    public class GridController : MonoBehaviour
    {
        public float nodeRadius;

        private List<Node> saveNodes;
        private float nodeDiameter;

        void Awake()
        {
            saveNodes = new List<Node>();
            nodeDiameter = nodeRadius * 2;
        }

        /// <summary>
        /// Возвращает Node-ы соседних клеток
        /// </summary>
        /// <param name="node">Проверяемая клетка</param>
        /// <returns>Клетки по соседству</returns>
        public Node[] GetNeighbours(Node node)
        {
            Node[] nodes = new Node[26];

            // Верхние Nodes
            nodes[0] = NodeFromWorldPosition(node.position + Vector3.up * nodeDiameter);
            nodes[1] = NodeFromWorldPosition(node.position + Vector3.up * nodeDiameter + Vector3.forward * nodeDiameter);
            nodes[2] = NodeFromWorldPosition(node.position + Vector3.up * nodeDiameter + Vector3.back * nodeDiameter);
            nodes[3] = NodeFromWorldPosition(node.position + Vector3.up * nodeDiameter + Vector3.left * nodeDiameter);
            nodes[4] = NodeFromWorldPosition(node.position + Vector3.up * nodeDiameter + Vector3.right * nodeDiameter);
            nodes[5] = NodeFromWorldPosition(node.position + Vector3.up * nodeDiameter + Vector3.forward * nodeDiameter + Vector3.left * nodeDiameter);
            nodes[6] = NodeFromWorldPosition(node.position + Vector3.up * nodeDiameter + Vector3.forward * nodeDiameter + Vector3.right * nodeDiameter);
            nodes[7] = NodeFromWorldPosition(node.position + Vector3.up * nodeDiameter + Vector3.back * nodeDiameter + Vector3.left * nodeDiameter);
            nodes[8] = NodeFromWorldPosition(node.position + Vector3.up * nodeDiameter + Vector3.back * nodeDiameter + Vector3.right * nodeDiameter);

            // Боковые Nodes
            nodes[9] = NodeFromWorldPosition(node.position + Vector3.forward * nodeDiameter);
            nodes[10] = NodeFromWorldPosition(node.position + Vector3.back * nodeDiameter);
            nodes[11] = NodeFromWorldPosition(node.position + Vector3.left * nodeDiameter);
            nodes[12] = NodeFromWorldPosition(node.position + Vector3.right * nodeDiameter);

            // Боковые угловые Nodes
            nodes[13] = NodeFromWorldPosition(node.position + Vector3.forward * nodeDiameter + Vector3.left * nodeDiameter);
            nodes[14] = NodeFromWorldPosition(node.position + Vector3.forward * nodeDiameter + Vector3.right * nodeDiameter);
            nodes[15] = NodeFromWorldPosition(node.position + Vector3.back * nodeDiameter + Vector3.left * nodeDiameter);
            nodes[16] = NodeFromWorldPosition(node.position + Vector3.back * nodeDiameter + Vector3.right * nodeDiameter);

            // Нижние Nodes
            nodes[17] = NodeFromWorldPosition(node.position + Vector3.down * nodeDiameter);
            nodes[18] = NodeFromWorldPosition(node.position + Vector3.down * nodeDiameter + Vector3.forward * nodeDiameter);
            nodes[19] = NodeFromWorldPosition(node.position + Vector3.down * nodeDiameter + Vector3.back * nodeDiameter);
            nodes[20] = NodeFromWorldPosition(node.position + Vector3.down * nodeDiameter + Vector3.left * nodeDiameter);
            nodes[21] = NodeFromWorldPosition(node.position + Vector3.down * nodeDiameter + Vector3.right * nodeDiameter);
            nodes[22] = NodeFromWorldPosition(node.position + Vector3.down * nodeDiameter + Vector3.forward * nodeDiameter + Vector3.left * nodeDiameter);
            nodes[23] = NodeFromWorldPosition(node.position + Vector3.down * nodeDiameter + Vector3.forward * nodeDiameter + Vector3.right * nodeDiameter);
            nodes[24] = NodeFromWorldPosition(node.position + Vector3.down * nodeDiameter + Vector3.back * nodeDiameter + Vector3.left * nodeDiameter);
            nodes[25] = NodeFromWorldPosition(node.position + Vector3.down * nodeDiameter + Vector3.back * nodeDiameter + Vector3.right * nodeDiameter);

            return nodes;
        }

        /// <summary>
        /// Находит или создаёт Node соответствующий заданной позиции
        /// </summary>
        /// <param name="position">Позиция проверяемого Node</param>
        /// <returns>Node соответствующий заданной позиции</returns>
        public Node NodeFromWorldPosition(Vector3 position)
        {
            if(saveNodes.Count == 0) return CreateNewNode(position);
            // Пытается найти Node по заданной позиции
            Node node = saveNodes.Find((x) => x.position == CorrectPosition(position));

            if (node == null)
            {
                return CreateNewNode(position);
            }
            else
            {
                return node;
            }
        }

        /// <summary>
        /// Cоздавёт и возвращает Node c позицией равной позиции будущего нода
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        private Node CreateNewNode(Vector3 position)
        {
            Node node;
            if (saveNodes.Count == 0)
            {
                node = new Node(position);
                saveNodes.Add(node);
                return node;
            }

            position = CorrectPosition(position);
            node = FindingNearestNode(position);

            if (node.position != position)
            {
                node = new Node(position);
                saveNodes.Add(node);
            }
            return node;
        }

        private Vector3 CorrectPosition(Vector3 finishPosition)
        {
            if (saveNodes.Count == 0) return finishPosition;

            Vector3 position = FindingNearestNode(finishPosition).position;

            if(position.x < finishPosition.x)
            {
                while (position.x < finishPosition.x)
                {
                    position.x += nodeDiameter;
                }
            }
            else if(position.x > finishPosition.x)
            {
                while (position.x > finishPosition.x)
                {
                    position.x -= nodeDiameter;
                }
            }

            if (position.y < finishPosition.y)
            {
                while (position.y < finishPosition.y)
                {
                    position.y += nodeDiameter;
                }
            }
            else if (position.y > finishPosition.y)
            {
                while (position.y > finishPosition.y)
                {
                    position.y -= nodeDiameter;
                }
            }

            if (position.z < finishPosition.z)
            {
                while (position.z < finishPosition.z)
                {
                    position.z += nodeDiameter;
                }
            }
            else if (position.z > finishPosition.z)
            {
                while (position.z > finishPosition.z)
                {
                    position.z -= nodeDiameter;
                }
            }

            return position;
        }

        private Node FindingNearestNode(Vector3 position)
        {
            return (from value in saveNodes
                    orderby Mathf.Abs(value.position.x - position.x) +
                    Mathf.Abs(value.position.y - position.y) +
                    Mathf.Abs(value.position.z - position.z)
                    select value).First();
        }
    }
}
