using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AgentStruct
{
    public float speed;
    public GameObject prefab;
    public Transform agent;
    public List<Node> path;
    public int targetIndex;
    public int oldTargetIndex;
}
