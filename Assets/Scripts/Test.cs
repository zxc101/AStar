using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public bool IsShowTransformNode;
    public bool IsShowNeighboursNode;

    private static New.GridController grid;

    void Start()
    {
        grid = GameObject.Find("NewGridController").GetComponent<New.GridController>();
        foreach (Node n in grid.GetNeighbours(grid.NodeFromWorldPosition(transform.position)))
        {
            print(n.position);
        }
    }

    //private void OnDrawGizmos()
    //{
    //    grid = GameObject.Find("NewGridController").GetComponent<New.GridController>();
    //    foreach (Node n in grid.GetNeighbours(grid.NodeFromWorldPosition(transform.position)))
    //    {
    //        Gizmos.color = Color.cyan;
    //        Gizmos.DrawCube(n.position, Vector3.one);
    //    }
    //}
}
