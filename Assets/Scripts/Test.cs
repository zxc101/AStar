using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    //public Transform seeker;
    //public Transform target;

    private static New.GridController grid;

    private void Awake()
    {
        grid = GameObject.Find("NewGridController").GetComponent<New.GridController>();
    }
}
