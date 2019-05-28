using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private static New.GridController grid;
    private void Start()
    {
        List<Node> list = new List<Node> { new Node(Vector3.zero) };
        //print(IsHaveNode(list, new Node(Vector3.zero)));
        ////////////////////////////////////

        //Node n1 = new Node(Vector3.zero);
        //Node n2 = new Node(Vector3.zero);

        //n1.gCost = 1;
        //n1.hCost = 1;
        //n2.gCost = 0;
        //n2.hCost = 0;

        //print(n1.CompareTo(n2));

        ///////////////////////////////

        //grid = GameObject.Find("NewGridController").GetComponent<New.GridController>();
        //Node seekerNode = grid.NodeFromWorldPosition(transform.position);
        //Node targetNode = grid.NodeFromWorldPosition(new Vector3(3.4f, -1.2f, 0.6f));
        //print(targetNode.position);
    }

    private static bool IsHaveNode(List<Node> list, Node n)
    {
        //return list.Contains(n);
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].position == n.position)
            {
                return true;
            }
        }
        return false;
        //Node node = list.Find((x) => x == grid.NodeFromWorldPosition(n.position));
        //return node != null;
    }

    //private void Start()
    //{
    //    List<int> arr = new List<int> { 170, 45, 75, 90, 802, 24, 2, 66 };
    //    int n = arr.Count;
    //    radixsort(arr, n);
    //    print(arr, n);
    //}

    //private int getMax(List<int> arr, int n)
    //{
    //    int mx = arr[0];
    //    for (int i = 1; i < n; i++)
    //        if (arr[i] > mx)
    //            mx = arr[i];
    //    return mx;
    //}

    //private void countSort(List<int> arr, int n, int exp)
    //{
    //    int[] output = new int[n];
    //    int i;
    //    int[] count = new int[10];

    //    for (i = 0; i < 10; i++)
    //        count[i] = 0;

    //    for (i = 0; i < n; i++)
    //        count[(arr[i] / exp) % 10]++;

    //    for (i = 1; i < 10; i++)
    //        count[i] += count[i - 1];

    //    for (i = n - 1; i >= 0; i--)
    //    {
    //        output[count[(arr[i] / exp) % 10] - 1] = arr[i];
    //        count[(arr[i] / exp) % 10]--;
    //    }

    //    for (i = 0; i < n; i++)
    //        arr[i] = output[i];
    //}

    //private void radixsort(List<int> arr, int n)
    //{
    //    int m = getMax(arr, n);

    //    for (int exp = 1; m / exp > 0; exp *= 10)
    //        countSort(arr, n, exp);
    //}

    //private void print(List<int> arr, int n)
    //{
    //    for (int i = 0; i < n; i++)
    //        print(arr[i] + " ");
    //}
}
