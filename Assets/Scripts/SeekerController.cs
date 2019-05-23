using System.Collections;
using UnityEngine;

public class SeekerController : MonoBehaviour
{
    [SerializeField] private GameObject seeker;
    [SerializeField] private Transform agent;
    [SerializeField] private int speed = 1;
    [SerializeField] private int countCrossingNodes;

    private SeekerStruct currentSeeker;

    private TargetController targetController;
    private GridController gridController;
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
        targetController = transform.parent.Find("TargetController").GetComponent<TargetController>();
        gridController = transform.parent.Find("GridController").GetComponent<GridController>();
    }

    public void SpawnAgent()
    {
        if (currentSeeker.prefab == null)
        {
            currentSeeker.prefab = Instantiate(seeker, new Vector3(Random.Range(-3.1f, 2.4f), 1.8f, Random.Range(3, 3.7f)), Quaternion.Euler(0, 180, 0));
            if (Physics.Raycast(currentSeeker.prefab.transform.position, -Vector3.up, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Frame")))
            {
                currentSeeker.prefab.transform.position = Vector3.right * currentSeeker.prefab.transform.position.x + Vector3.up * (hit.transform.position.y + currentSeeker.prefab.transform.localScale.y * 3 / 4 ) + Vector3.forward * currentSeeker.prefab.transform.position.z;
            }
            currentSeeker.oldTargetIndex = -1;
            currentSeeker.seeker = currentSeeker.prefab.transform.Find("Rig_Cat_Lite/Master/BackBone_03/BackBone_02/BackBone_01") != null ?
                currentSeeker.prefab.transform.Find("Rig_Cat_Lite/Master/BackBone_03/BackBone_02/BackBone_01") : currentSeeker.prefab.transform;
            currentSeeker.speed = speed;
            gridController.GridUpdate();
        }
    }

    public void DestroyAgent()
    {
        if(currentSeeker.prefab != null)
        {
            currentSeeker.speed = 0;
            currentSeeker.targetIndex = 0;
            currentSeeker.oldTargetIndex = 0;
            currentSeeker.path = null;
            currentSeeker.seeker = null;
            Destroy(currentSeeker.prefab);
        }
    }

    public void StartMove()
    {
        currentSeeker.targetIndex = Random.Range(0, targetController.SaveTargets.Count);

        while (currentSeeker.oldTargetIndex == currentSeeker.targetIndex)
            if (targetController.SaveTargets.Count <= 1)
                return;
            else
                currentSeeker.targetIndex = Random.Range(0, targetController.SaveTargets.Count);

        currentSeeker.path = New.Pathfinder.FindPath(currentSeeker.seeker.position, targetController.SaveTargets[currentSeeker.targetIndex].transform.position);
        StopCoroutine(FollowPath());
        StartCoroutine(FollowPath());
        currentSeeker.oldTargetIndex = currentSeeker.targetIndex;
    }

    private IEnumerator FollowPath()
    {
        int targetIndex = 0;
        Vector3 currentWaypoint = currentSeeker.path[targetIndex].position;

        while (true)
        {
            if (currentSeeker.prefab.transform.position == currentWaypoint)
            {
                targetIndex++;
                if (targetIndex >= currentSeeker.path.Count)
                {
                    yield break;
                }
                currentWaypoint = currentSeeker.path[targetIndex].position;
            }
            currentSeeker.prefab.transform.position = Vector3.MoveTowards(currentSeeker.prefab.transform.position, currentWaypoint, currentSeeker.speed * Time.deltaTime);
            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        if(currentSeeker.path != null)
        {
            foreach(Node n in currentSeeker.path)
            {
                Gizmos.color = new Color(255, 247, 157);
                Gizmos.DrawCube(n.position, Vector3.one/10);
            }
        }
    }
}
