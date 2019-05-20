using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentController : MonoBehaviour
{
    [SerializeField] private GameObject agent;
    [SerializeField] private int speed = 1;
    [SerializeField] private int countCrossingNodes;

    private AgentStruct currentAgent;

    private GridController gridController;
    private TargetController targetController;
    private RaycastHit hit;

    private void OnValidate()
    {
        if (speed < 1)
        {
            speed = 1;
        }
        if(countCrossingNodes < 0)
        {
            countCrossingNodes = 0;
        }
    }

    void Start()
    {
        gridController = transform.parent.Find("GridController").GetComponent<GridController>();
        targetController = transform.parent.Find("TargetController").GetComponent<TargetController>();
    }

    public void SpawnAgent()
    {
        if (currentAgent.prefab == null)
        {
            currentAgent.prefab = Instantiate(agent, new Vector3(Random.Range(-3.1f, 2.4f), 1.8f, Random.Range(3, 3.7f)), Quaternion.Euler(0, 180, 0));
            if (Physics.Raycast(currentAgent.prefab.transform.position, -Vector3.up, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Frame")))
            {
                currentAgent.prefab.transform.position = Vector3.right * currentAgent.prefab.transform.position.x + Vector3.up * (hit.transform.position.y + currentAgent.prefab.transform.localScale.y * 3 / 4 ) + Vector3.forward * currentAgent.prefab.transform.position.z;
            }
            currentAgent.oldTargetIndex = -1;
            currentAgent.agent = currentAgent.prefab.transform.Find("Rig_Cat_Lite/Master/BackBone_03/BackBone_02/BackBone_01") != null ?
                currentAgent.prefab.transform.Find("Rig_Cat_Lite/Master/BackBone_03/BackBone_02/BackBone_01") : currentAgent.prefab.transform;
            currentAgent.speed = speed;

            gridController.GridUpdate();
        }
    }

    public void DestroyAgent()
    {
        if(currentAgent.prefab != null)
        {
            currentAgent.speed = 0;
            currentAgent.targetIndex = 0;
            currentAgent.oldTargetIndex = 0;
            currentAgent.path = null;
            currentAgent.agent = null;
            Destroy(currentAgent.prefab);
        }
    }

    public void StartMove()
    {
        currentAgent.targetIndex = Random.Range(0, targetController.SaveTargets.Count);

        while (currentAgent.oldTargetIndex == currentAgent.targetIndex)
            if (targetController.SaveTargets.Count <= 1)
                return;
            else
                currentAgent.targetIndex = Random.Range(0, targetController.SaveTargets.Count);

        currentAgent.path = Pathfinder.FindPath(currentAgent.agent.position, targetController.SaveTargets[currentAgent.targetIndex].transform.position, countCrossingNodes);
        StopCoroutine(FollowPath());
        StartCoroutine(FollowPath());
        currentAgent.oldTargetIndex = currentAgent.targetIndex;
    }

    private IEnumerator FollowPath()
    {
        int targetIndex = 0;
        Vector3 currentWaypoint = currentAgent.path[targetIndex].worldPosition;

        while (true)
        {
            if (currentAgent.prefab.transform.position == currentWaypoint)
            {
                targetIndex++;
                if (targetIndex >= currentAgent.path.Count)
                {
                    yield break;
                }
                currentWaypoint = currentAgent.path[targetIndex].worldPosition;
            }
            currentAgent.prefab.transform.position = Vector3.MoveTowards(currentAgent.prefab.transform.position, currentWaypoint, currentAgent.speed * Time.deltaTime);
            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        if(currentAgent.path != null)
        {
            foreach(Node n in currentAgent.path)
            {
                Gizmos.color = new Color(255, 247, 157);
                Gizmos.DrawCube(n.worldPosition, Vector3.one/10);
            }
        }
    }
}
