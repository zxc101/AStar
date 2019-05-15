using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    public float speed;
    [HideInInspector] public Transform seeker;

    private List<Node> path;
    private TargetBase targetBase;
    private int targetIndex;
    private int oldTargetIndex;

    private void OnValidate()
    {
        seeker = transform.Find("Rig_Cat_Lite").
                           Find("Master").
                           Find("BackBone_03").
                           Find("BackBone_02").
                           Find("BackBone_01");

        speed = 5;

        if (speed < 0)
        {
            speed = 0;
        }
    }

    private void Start()
    {
        targetBase = GameObject.Find("TargetBase").GetComponent<TargetBase>();
        oldTargetIndex = -1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            targetIndex = Random.Range(0, targetBase.SaveTargets.Length);
            
            while (oldTargetIndex == targetIndex)
                if(targetBase.SaveTargets.Length <= 1)
                    return;
                else
                    targetIndex = Random.Range(0, targetBase.SaveTargets.Length);

            path = Pathfinder.FindPath(seeker.position, targetBase.SaveTargets[targetIndex].transform.position);
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
            oldTargetIndex = targetIndex;
        }
    }

    private IEnumerator FollowPath()
    {
        int targetIndex = 0;
        Vector3 currentWaypoint = path[targetIndex].worldPosition;
        //Vector3 lastWaypoint = path[path.Count - 1].worldPosition;

        while (true)
        {
            if (transform.position == currentWaypoint)
            {
                targetIndex++;
                if (targetIndex >= path.Count)
                {
                    //if (Vector3.right * target.position.x != Vector3.right * lastWaypoint.x ||
                    //Vector3.forward * target.position.z != Vector3.forward * lastWaypoint.z)
                    //{
                    //    path = pathfinder.FindPath(seeker.position, target.position);
                    //    //Debug.Log(path.Count);
                    //    if (path.Count == 0)
                    //    {
                    //        //Debug.Log(path.Count);
                    //        StopCoroutine("FollowPath");
                    //        yield break;
                    //    }
                    //    StartCoroutine("FollowPath");
                    //}
                    yield break;
                }
                currentWaypoint = path[targetIndex].worldPosition;
            }

            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
            yield return null;
        }
    }
}
