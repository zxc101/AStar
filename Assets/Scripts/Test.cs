using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    //public bool IsShowTransformNode;
    //public bool IsShowNeighboursNode;
    public Transform seeker;
    public Transform target;

    private static New.GridController grid;

    //private void OnValidate()
    //{
    //    grid = GameObject.Find("NewGridController").GetComponent<New.GridController>();
    //}

    void Start()
    {
        grid = GameObject.Find("NewGridController").GetComponent<New.GridController>();
        Node seekerNode = grid.NodeFromWorldPosition(seeker.position);
        Node targetNode = grid.NodeFromWorldPosition(target.position);
        print($"Seeker: {seekerNode.position}");
        print($"Target: {targetNode.position}");

        ///////////////////////////////////////////////////////////////////////////////////

        //grid = GameObject.Find("NewGridController").GetComponent<New.GridController>();
        //foreach (Node n in grid.GetNeighbours(grid.NodeFromWorldPosition(transform.position)))
        //{
        //    print(n.position);
        //}
    }

    //private void OnDrawGizmos()
    //{
        //Gizmos.color = Color.blue;
        //Gizmos.DrawCube(grid.NodeFromWorldPosition(seeker.position).position, Vector3.one * 0.3f);
        //Gizmos.color = Color.yellow;
        //Gizmos.DrawCube(grid.NodeFromWorldPosition(target.position).position, Vector3.one * 0.3f);
        //    grid = GameObject.Find("NewGridController").GetComponent<New.GridController>();
        //    foreach (Node n in grid.GetNeighbours(grid.NodeFromWorldPosition(transform.position)))
        //    {
        //        Gizmos.color = Color.cyan;
        //        Gizmos.DrawCube(n.position, Vector3.one);
        //    }
    //}
}
