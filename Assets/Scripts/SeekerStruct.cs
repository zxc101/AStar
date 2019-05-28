using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SeekerStruct
{
    public float speed;
    public GameObject prefab;
    public Vector3 seeker;
    public List<Node> path;
    public int targetIndex;
    public int oldTargetIndex;
}
